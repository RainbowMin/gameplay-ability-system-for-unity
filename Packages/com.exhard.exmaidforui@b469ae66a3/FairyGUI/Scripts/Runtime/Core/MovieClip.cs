﻿using UnityEngine;

namespace FairyGUI
{
    /// <summary>
    /// </summary>
    public class MovieClip : Image
    {
        private int _end;
        private int _endAt;
        private int _frame;
        private int _frameCount;

        private float _frameElapsed; //当前帧延迟

        private Frame[] _frames;

        private EventListener _onPlayEnd;
        private bool _playing;
        private int _repeatedCount;
        private bool _reversed;
        private int _start;
        private int _status; //0-none, 1-next loop, 2-ending, 3-ended
        private readonly TimerCallback _timerDelegate;
        private int _times;

        /// <summary>
        ///     Whether to ignore Unity time scale.
        /// </summary>
        public bool ignoreEngineTimeScale;

        /// <summary>
        /// </summary>
        public float interval;

        /// <summary>
        /// </summary>
        public float repeatDelay;

        /// <summary>
        /// </summary>
        public bool swing;

        /// <summary>
        /// </summary>
        public float timeScale;

        /// <summary>
        /// </summary>
        public MovieClip()
        {
            interval = 0.1f;
            _playing = true;
            _timerDelegate = OnTimer;
            timeScale = 1;
            ignoreEngineTimeScale = false;

            if (Application.isPlaying)
            {
                onAddedToStage.Add(OnAddedToStage);
                onRemovedFromStage.Add(OnRemoveFromStage);
            }

            SetPlaySettings();
        }

        /// <summary>
        /// </summary>
        public EventListener onPlayEnd => _onPlayEnd ?? (_onPlayEnd = new EventListener(this, "onPlayEnd"));

        /// <summary>
        /// </summary>
        public Frame[] frames
        {
            get => _frames;
            set
            {
                _frames = value;
                _scale9Grid = null;
                _scaleByTile = false;

                if (_frames == null)
                {
                    _frameCount = 0;
                    graphics.texture = null;
                    CheckTimer();
                    return;
                }

                _frameCount = frames.Length;

                if (_end == -1 || _end > _frameCount - 1)
                    _end = _frameCount - 1;
                if (_endAt == -1 || _endAt > _frameCount - 1)
                    _endAt = _frameCount - 1;

                if (_frame < 0 || _frame > _frameCount - 1)
                    _frame = _frameCount - 1;

                InvalidateBatchingState();

                _frameElapsed = 0;
                _repeatedCount = 0;
                _reversed = false;

                DrawFrame();
                CheckTimer();
            }
        }

        /// <summary>
        /// </summary>
        public bool playing
        {
            get => _playing;
            set
            {
                if (_playing != value)
                {
                    _playing = value;
                    CheckTimer();
                }
            }
        }

        /// <summary>
        /// </summary>
        public int frame
        {
            get => _frame;
            set
            {
                if (_frame != value)
                {
                    if (_frames != null && value >= _frameCount)
                        value = _frameCount - 1;

                    _frame = value;
                    _frameElapsed = 0;
                    DrawFrame();
                }
            }
        }

        /// <summary>
        /// </summary>
        public void Rewind()
        {
            _frame = 0;
            _frameElapsed = 0;
            _reversed = false;
            _repeatedCount = 0;
            DrawFrame();
        }

        /// <summary>
        /// </summary>
        /// <param name="anotherMc"></param>
        public void SyncStatus(MovieClip anotherMc)
        {
            _frame = anotherMc._frame;
            _frameElapsed = anotherMc._frameElapsed;
            _reversed = anotherMc._reversed;
            _repeatedCount = anotherMc._repeatedCount;
            DrawFrame();
        }

        /// <summary>
        /// </summary>
        /// <param name="time"></param>
        public void Advance(float time)
        {
            var beginFrame = _frame;
            var beginReversed = _reversed;
            var backupTime = time;
            while (true)
            {
                var tt = interval + _frames[_frame].addDelay;
                if (_frame == 0 && _repeatedCount > 0)
                    tt += repeatDelay;
                if (time < tt)
                {
                    _frameElapsed = 0;
                    break;
                }

                time -= tt;

                if (swing)
                {
                    if (_reversed)
                    {
                        _frame--;
                        if (_frame <= 0)
                        {
                            _frame = 0;
                            _repeatedCount++;
                            _reversed = !_reversed;
                        }
                    }
                    else
                    {
                        _frame++;
                        if (_frame > _frameCount - 1)
                        {
                            _frame = Mathf.Max(0, _frameCount - 2);
                            _repeatedCount++;
                            _reversed = !_reversed;
                        }
                    }
                }
                else
                {
                    _frame++;
                    if (_frame > _frameCount - 1)
                    {
                        _frame = 0;
                        _repeatedCount++;
                    }
                }

                if (_frame == beginFrame && _reversed == beginReversed) //走了一轮了
                {
                    var roundTime = backupTime - time; //这就是一轮需要的时间
                    time -= Mathf.FloorToInt(time / roundTime) * roundTime; //跳过
                }
            }

            DrawFrame();
        }

        /// <summary>
        /// </summary>
        public void SetPlaySettings()
        {
            SetPlaySettings(0, -1, 0, -1);
        }

        /// <summary>
        ///     从start帧开始，播放到end帧（-1表示结尾），重复times次（0表示无限循环），循环结束后，停止在endAt帧（-1表示参数end）
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="times"></param>
        /// <param name="endAt"></param>
        public void SetPlaySettings(int start, int end, int times, int endAt)
        {
            _start = start;
            _end = end;
            if (_end == -1 || _end > _frameCount - 1)
                _end = _frameCount - 1;
            _times = times;
            _endAt = endAt;
            if (_endAt == -1)
                _endAt = _end;
            _status = 0;
            frame = start;
        }

        private void OnAddedToStage()
        {
            if (_playing && _frameCount > 0)
                Timers.inst.AddUpdate(_timerDelegate);
        }

        private void OnRemoveFromStage()
        {
            Timers.inst.Remove(_timerDelegate);
        }

        private void CheckTimer()
        {
            if (!Application.isPlaying)
                return;

            if (_playing && _frameCount > 0 && stage != null)
                Timers.inst.AddUpdate(_timerDelegate);
            else
                Timers.inst.Remove(_timerDelegate);
        }

        private void OnTimer(object param)
        {
            if (!_playing || _frameCount == 0 || _status == 3)
                return;

            float dt;
            if (ignoreEngineTimeScale)
            {
                dt = Time.unscaledDeltaTime;
                if (dt > 0.1f)
                    dt = 0.1f;
            }
            else
            {
                dt = Time.deltaTime;
            }

            if (timeScale != 1)
                dt *= timeScale;

            _frameElapsed += dt;
            var tt = interval + _frames[_frame].addDelay;
            if (_frame == 0 && _repeatedCount > 0)
                tt += repeatDelay;
            if (_frameElapsed < tt)
                return;

            _frameElapsed -= tt;
            if (_frameElapsed > interval)
                _frameElapsed = interval;

            if (swing)
            {
                if (_reversed)
                {
                    _frame--;
                    if (_frame <= 0)
                    {
                        _frame = 0;
                        _repeatedCount++;
                        _reversed = !_reversed;
                    }
                }
                else
                {
                    _frame++;
                    if (_frame > _frameCount - 1)
                    {
                        _frame = Mathf.Max(0, _frameCount - 2);
                        _repeatedCount++;
                        _reversed = !_reversed;
                    }
                }
            }
            else
            {
                _frame++;
                if (_frame > _frameCount - 1)
                {
                    _frame = 0;
                    _repeatedCount++;
                }
            }

            if (_status == 1) //new loop
            {
                _frame = _start;
                _frameElapsed = 0;
                _status = 0;
                DrawFrame();
            }
            else if (_status == 2) //ending
            {
                _frame = _endAt;
                _frameElapsed = 0;
                _status = 3; //ended
                DrawFrame();

                DispatchEvent("onPlayEnd", null);
            }
            else
            {
                DrawFrame();
                if (_frame == _end)
                {
                    if (_times > 0)
                    {
                        _times--;
                        if (_times == 0)
                            _status = 2; //ending
                        else
                            _status = 1; //new loop
                    }
                    else if (_start != 0)
                    {
                        _status = 1; //new loop
                    }
                }
            }
        }

        private void DrawFrame()
        {
            if (_frameCount > 0)
            {
                var frame = _frames[_frame];
                graphics.texture = frame.texture;
            }
        }

        /// <summary>
        /// </summary>
        public class Frame
        {
            public float addDelay;
            public NTexture texture;
        }
    }
}