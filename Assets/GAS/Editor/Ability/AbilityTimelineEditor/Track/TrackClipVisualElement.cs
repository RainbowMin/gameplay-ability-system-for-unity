using System;
using GAS.Runtime.Ability.AbilityTimeline;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace GAS.Editor.Ability.AbilityTimelineEditor
{
    public class TrackClipVisualElement:VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TrackClipVisualElement, UxmlTraits> { }
        protected string ItemAssetGUID => "3197d239f4ce79b41b2278ecea5aaab8";

        private VisualElement _outsideBox;
        private Label _itemLabel;
        private VisualElement _overLine;
        private VisualElement _selectedBottomLine;
        private VisualElement _mainArea;
        private PointerIMGUIContainer _leftResizeArea;
        private IMGUIContainer _rightResizeArea;

        private DragAreaManipulator _mainAreaDragAreaManipulator;
        private DragAreaManipulator _leftDragAreaManipulator;
        private DragAreaManipulator _rightDragAreaManipulator;
            
        public Label ItemLabel => _itemLabel;
        public VisualElement OverLine => _overLine;

        private TrackClipBase _clip;
        private float FrameUnitWidth=>_clip.FrameUnitWidth;
        private int StartFrameIndex=>_clip.StartFrameIndex;
        private int EndFrameIndex => _clip.EndFrameIndex;
        private int DurationFrame => _clip.DurationFrame;

        public bool Selected { get; private set; }
        public bool Hovered { get; private set; }

        private TimerShaftView TimerShaftView => AbilityTimelineEditorWindow.Instance.TimerShaftView;
        
        public TrackClipVisualElement()
        {
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(ItemAssetGUID));
            visualTree.CloneTree(this);
            AddToClassList("clip");

            _outsideBox = this.Q<VisualElement>("OutsideBox");
            _itemLabel = this.Q<Label>("ItemLabel");
            _overLine = this.Q<VisualElement>("OverLine");
            _selectedBottomLine = this.Q<VisualElement>("SelectedBottomLine");
            _mainArea = this.Q<VisualElement>("Main");
            _leftResizeArea = this.Q<PointerIMGUIContainer>("LeftResizeArea");
            _rightResizeArea = this.Q<IMGUIContainer>("RightResizeArea");


            _mainAreaDragAreaManipulator = new DragAreaManipulator(MouseCursorType.None, OnMainMouseMove,
                OnMainMouseDown, OnMainMouseUp);
            _mainArea.AddManipulator(_mainAreaDragAreaManipulator);

            _leftDragAreaManipulator = new DragAreaManipulator(MouseCursorType.ResizeHorizontal, OnLeftResizeDragMove,
                OnLeftResizeDragStart, OnLeftResizeDragEnd);
            _leftResizeArea.AddManipulator(_leftDragAreaManipulator);

            _rightDragAreaManipulator = new DragAreaManipulator(MouseCursorType.ResizeHorizontal, OnRightResizeDragMove,
                OnRightResizeDragStart, OnRightResizeDragEnd);
            _rightResizeArea.AddManipulator(_rightDragAreaManipulator);
            
            _mainArea.AddManipulator(new ContextualMenuManipulator(OnContextMenu));
        }

        private void OnContextMenu(ContextualMenuPopulateEvent obj)
        {
            obj.menu.AppendAction("Delete Clip", action => _clip.Delete());
        }

        public void InitClipInfo(TrackClipBase trackClipBase)
        {
            _clip = trackClipBase;
        }
        
        #region Main Area Mouse Event
        
        private Func<float,int> getMinStartFrameIndex;
        private Func<float,int> getMaxEndFrameIndex;
        private float _lastMainDragStartPos;
        private float _newStartFramePos;
        private int NewStartFrame => (int)_newStartFramePos;
        
        public void RegisterFuncGetMinStartFrameIndex (Func<float,int> func)
        {
            getMinStartFrameIndex = func;
        }
        
        public void RegisterFuncGetMaxEndFrameIndex (Func<float,int> func)
        {
            getMaxEndFrameIndex = func;
        }
        
        protected void OnMainMouseDown(PointerDownEvent evt)
        {
            _lastMainDragStartPos = StartFrameIndex;
            OnSelect();
        }
        
        protected void OnMainMouseUp()
        {
            if(TimerShaftView.ShowDragItemPreview == false) return;
            
            OnMainAreaApplyDrag();
            TimerShaftView.ShowDragItemPreview = false;
            TimerShaftView.DottedLineFrameIndex = -1;
        }

        private void OnMainMouseMove(Vector2 delta)
        {
            var offsetFrame = delta.x / FrameUnitWidth;
            _newStartFramePos = _lastMainDragStartPos + offsetFrame;
            if (offsetFrame == 0 || _newStartFramePos < 0) return;
            int minFrame = getMinStartFrameIndex?.Invoke(_lastMainDragStartPos) ?? 0;
            int maxFrame = getMaxEndFrameIndex?.Invoke(_lastMainDragStartPos) ?? AbilityTimelineEditorWindow.Instance.AbilityAsset.MaxFrameCount;
            if (NewStartFrame >= minFrame && NewStartFrame+DurationFrame <= maxFrame)
            {
                var mainContent=TimerShaftView.MainContent;
                var bound = _mainArea.worldBound;
                var rectOriginX = NewStartFrame * FrameUnitWidth - AbilityTimelineEditorWindow.Instance.CurrentFramePos;
                bound.y = bound.y - mainContent.worldBound.y + bound.height;
                bound.x = Mathf.Clamp(rectOriginX, 0, parent.worldBound.width);
                if (rectOriginX < 0)
                    bound.width += rectOriginX;
                if (bound.width + bound.x > mainContent.worldBound.width - 8)
                    bound.width = mainContent.worldBound.width - bound.x - 8; // 8 = 滑动条宽度
                TimerShaftView.ShowDragItemPreview = true;
                TimerShaftView.DragItemPreviewRect = bound;
                TimerShaftView.DottedLineFrameIndex = NewStartFrame;
            }
        }

        private void OnMainAreaApplyDrag()
        {
            int minFrame = getMinStartFrameIndex?.Invoke(_lastMainDragStartPos) ?? 0;
            int maxFrame = getMaxEndFrameIndex?.Invoke(_lastMainDragStartPos) ?? AbilityTimelineEditorWindow.Instance.AbilityAsset.MaxFrameCount;
            var newStartFrame = Mathf.Clamp(NewStartFrame, minFrame, maxFrame - DurationFrame);
            if (newStartFrame == StartFrameIndex) return;
            
            _clip.UpdateClipDataStartFrame(newStartFrame);
            _clip.RefreshShow(FrameUnitWidth);
            
            AbilityTimelineEditorWindow.Instance.TimelineInspector.RefreshInspector();
        }
        
        #endregion
        
        #region Hover And Select

        protected static readonly Color NormalColor = new(0.3f, 0.3f, 0.5f, 0.75f);
        protected static readonly Color SelectColor = new(0.6f, 0.1f, 0.1f, 0.75f);
        protected static readonly Color OutsideBoxSelectColor = new Color(0.8f, 0.5f, 0.1f, 1f);
        protected static readonly Color OutsideBoxHoverColor =new Color(0.8f, 0.8f, 0.7f, 0.9f);
        
        private const int TipBoundingSize = 4;
        public bool InClipRect(Vector2 position)
        {
            var rect = _mainArea.worldBound;
            return rect.Contains(position);
        }

        public void SwitchBounding()
        {
            bool isShow = Hovered || Selected;
            if(isShow)
            {
                var color = Selected ? OutsideBoxSelectColor : OutsideBoxHoverColor;
                _outsideBox.style.backgroundColor = new StyleColor(color);
                _outsideBox.style.width = new StyleLength(_mainArea.worldBound.width+TipBoundingSize);
                _outsideBox.style.height = new StyleLength(_mainArea.worldBound.height+TipBoundingSize);
                _outsideBox.style.display = DisplayStyle.Flex;
                _outsideBox.MarkDirtyRepaint();
            }
            else
            {
                _outsideBox.style.display = DisplayStyle.None;
                _outsideBox.MarkDirtyRepaint();
            }
        }
        
        public void OnHover(bool value)
        {
            Hovered = value switch
            {
                true when !Hovered && !Selected => true,
                false when Hovered => false,
                _ => Hovered
            };
            SwitchBounding();
        }

        public virtual void OnSelect()
        {
            AbilityTimelineEditorWindow.Instance.SetInspector(_clip);
            Selected = true;
            _itemLabel.style.backgroundColor = SelectColor;
            SwitchBounding();
        }

        public virtual void OnUnSelect()
        {
            Selected = false;
            _itemLabel.style.backgroundColor = NormalColor;
            SwitchBounding();
        }

        #endregion

        #region Clip Resize Area
        
        private float _lastResizeDragStartPos;
        private float _newResizeStartFramePos;
        private int NewResizeStartFrame => (int)_newResizeStartFramePos;
        
        private float _lastResizeDragEndPos;
        private float _newResizeEndFramePos;
        private int NewResizeEndFrame => (int)_newResizeEndFramePos;
        
        private void OnLeftResizeDragMove(Vector2 delta)
        {
            var offsetFrame = delta.x / FrameUnitWidth;
            _newResizeStartFramePos = _lastResizeDragStartPos + offsetFrame;
            
            if (offsetFrame == 0 || _newResizeStartFramePos < 0 || NewResizeStartFrame + 1 >= EndFrameIndex) return;

            int minFrame = getMinStartFrameIndex?.Invoke(_lastMainDragStartPos) ?? 0;
            int maxFrame = EndFrameIndex - 1;
            if (NewResizeStartFrame >= minFrame && NewResizeStartFrame <= maxFrame)
            {
                TimerShaftView.DottedLineFrameIndex = NewResizeStartFrame;
            }
        }

        private void OnLeftResizeDragStart(PointerDownEvent evt)
        {
            _lastResizeDragStartPos = StartFrameIndex;
        }
        
        private void OnLeftResizeDragEnd()
        { 
            int lastEndFrame = EndFrameIndex;
            int minFrame = getMinStartFrameIndex?.Invoke(_lastMainDragStartPos) ?? 0;
            int maxFrame = EndFrameIndex - 1;
            var newStartFrame = Mathf.Clamp(NewResizeStartFrame, minFrame, maxFrame);
            
            _clip.UpdateClipDataStartFrame(newStartFrame);
            _clip.UpdateClipDataDurationFrame(lastEndFrame - _clip.StartFrameIndex);
            if (EndFrameIndex > AbilityTimelineEditorWindow.Instance.AbilityAsset.MaxFrameCount)
                AbilityTimelineEditorWindow.Instance.CurrentSelectFrameIndex = EndFrameIndex;
            _clip.RefreshShow(FrameUnitWidth);
            AbilityTimelineEditorWindow.Instance.SetInspector(_clip);
            TimerShaftView.DottedLineFrameIndex = -1;
            
            AbilityTimelineEditorWindow.Instance.TimelineInspector.RefreshInspector();
        }
        
        void OnRightResizeDragMove  (Vector2 delta)
        {
            var offsetFrame = delta.x / FrameUnitWidth;
            _newResizeEndFramePos = _lastResizeDragEndPos + offsetFrame;
            if (offsetFrame == 0 || _newResizeEndFramePos < 0 || NewResizeEndFrame - 1 <= StartFrameIndex) return;
            
            int maxFrame = getMaxEndFrameIndex?.Invoke(_lastMainDragStartPos) ?? AbilityTimelineEditorWindow.Instance.AbilityAsset.MaxFrameCount;
            int minFrame = _clip.StartFrameIndex + 1;
            if (NewResizeEndFrame >= minFrame && NewResizeEndFrame <= maxFrame)
            {
                TimerShaftView.DottedLineFrameIndex = NewResizeEndFrame;
            }
        }
        
        void OnRightResizeDragStart(PointerDownEvent evt)
        {
            _lastResizeDragEndPos = EndFrameIndex;
        }
        
        void OnRightResizeDragEnd()
        {
            int maxFrame = getMaxEndFrameIndex?.Invoke(_lastMainDragStartPos) ?? AbilityTimelineEditorWindow.Instance.AbilityAsset.MaxFrameCount;
            var newEndFrame = Mathf.Clamp(NewResizeEndFrame, StartFrameIndex + 1, maxFrame);
            _clip.UpdateClipDataDurationFrame(newEndFrame - _clip.StartFrameIndex);
            if (EndFrameIndex > AbilityTimelineEditorWindow.Instance.AbilityAsset.MaxFrameCount)
                AbilityTimelineEditorWindow.Instance.CurrentSelectFrameIndex = EndFrameIndex;
            _clip.RefreshShow(FrameUnitWidth);
            AbilityTimelineEditorWindow.Instance.SetInspector(_clip);
            TimerShaftView.DottedLineFrameIndex = -1;
            
            AbilityTimelineEditorWindow.Instance.TimelineInspector.RefreshInspector();
        }
        #endregion
    }
}
#endif