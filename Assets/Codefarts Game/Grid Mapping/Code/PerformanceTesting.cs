/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
#if PERFORMANCE
// ReSharper disable LoopCanBeConvertedToQuery

namespace Codefarts.GridMapping
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Provides a simple performance testing class that utilizes <see cref="Stopwatch"/>.
    /// </summary>
    /// <typeparam name="T">The type that will be used as the indexer.</typeparam>
    public class PerformanceTesting<T>
    {
        /// <summary>
        /// Provides a model that contains timer information.
        /// </summary>
        private class TimerModel
        {
            /// <summary>
            /// Used to record performance timings.
            /// </summary>
            public readonly Stopwatch Timer;

            /// <summary>
            /// Used to record how many times the <see cref="Timer"/> has been started.
            /// </summary>
            public int Count;

            /// <summary>
            /// Used to store the enabled state.
            /// </summary>
            private bool enabled;

            /// <summary>
            /// Gets or sets a value whether or not this timer if enabled.
            /// </summary>
            /// <remarks>If <see cref="Timer"/> has been started and Enabled is set to false the timer will be stopped.</remarks>
            public bool Enabled
            {
                get
                {
                    return this.enabled;
                }

                set
                {
                    this.enabled = value;

                    // be sure to stop the timer if disabled
                    if (!value && this.Timer.IsRunning)
                    {
                        this.Timer.Stop();
                    }
                }
            }

            /// <summary>
            /// Default constructor.
            /// </summary>
            public TimerModel()
            {
                this.Timer = new Stopwatch();
                this.enabled = true;
            }
        }

        /// <summary>
        /// Holds a reference to a singleton instance.
        /// </summary>
        private static PerformanceTesting<T> singleton;

        /// <summary>
        /// Used to store various timer information.
        /// </summary>
        private readonly Dictionary<T, TimerModel> timers;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PerformanceTesting()
        {
            this.timers = new Dictionary<T, TimerModel>();
        }

        /// <summary>
        /// Creates a new timer.
        /// </summary>
        /// <param name="key">The unique key for the timer.</param>
        /// <remarks>It is best to create the timer at the start of the application getting it ready for use.</remarks>
        public void Create(T key)
        {
            this.timers.Add(key, new TimerModel());
        }

        /// <summary>
        /// Creates a new timer for each key.
        /// </summary>
        /// <param name="keys">The unique keys for the timers.</param>
        /// <remarks>It is best to create the timer at the start of the application getting it ready for use.</remarks>
        public void Create(T[] keys)
        {
            foreach (var key in keys)
            {
                this.timers.Add(key, new TimerModel());
            }
        }

        /// <summary>
        /// Returns the total ticks that this timer has observed.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <returns>Returns a time value in ticks.</returns>
        /// <remarks>Will return -1 if the timer <see cref="key"/> is not found.</remarks>
        public long TotalTicks(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return -1;
            }
            var model = this.timers[key];
            return model.Timer.ElapsedTicks;
        }

        /// <summary>
        /// Returns the total ticks that this timer has observed.
        /// </summary>
        /// <param name="keys">The keys to the timers information.</param>
        /// <returns>Returns the sum of the time values in ticks.</returns>
        public long TotalTicks(T[] keys)
        {
            long total = 0;
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                total += model.Timer.ElapsedTicks;
            }

            return total;
        }

        /// <summary>
        /// Gets the start count.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <returns>The number of times the timer has started.</returns>
        /// <remarks>Will return -1 if the timer <see cref="key"/> is not found.</remarks>
        public int GetStartCount(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return -1;
            }
            var model = this.timers[key];
            return model.Count;
        }

        /// <summary>
        /// Gets the start count for specified timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <returns>The sum total of times all specified the timers have started.</returns>
        public int GetStartCount(T[] keys)
        {
            int total = 0;
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                total += model.Count;
            }

            return total;
        }

        /// <summary>
        /// Returns the total ticks that this timer has observed.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <returns>Returns a time value in milliseconds.</returns>
        /// <remarks>Will return -1 if the timer <see cref="key"/> is not found.</remarks>
        public long TotalMilliseconds(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return -1;
            }
            var model = this.timers[key];
            return model.Timer.ElapsedMilliseconds;
        }

        /// <summary>
        /// Returns the total ticks that this timer has observed.
        /// </summary>
        /// <param name="keys">The keys to the timers information.</param>
        /// <returns>Returns the sum of the time values in milliseconds.</returns>
        public long TotalMilliseconds(T[] keys)
        {
            long total = 0;
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                total += model.Timer.ElapsedMilliseconds;
            }

            return total;
        }

        /// <summary>
        /// Calculates the average time in ticks that elapsed while this timer was recording.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <returns>Returns the average time in ticks that elapsed between each start and stop.</returns>
        /// <remarks>Will return -1 if the timer <see cref="key"/> is not found.</remarks>
        public long AverageTicks(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return -1;
            }
            var model = this.timers[key];
            if (model.Count == 0)
            {
                return 0;
            }
            return model.Timer.ElapsedTicks / model.Count;
        }

        /// <summary>
        /// Calculates the total average time in ticks that elapsed while the specified timers were recording.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <returns>Returns the total average time in ticks that elapsed between each start and stop for all the specified timers.</returns>
        public long AverageTicks(T[] keys)
        {
            long total = 0;
            var count = 0;
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                total += model.Timer.ElapsedTicks;
                count += model.Count;
            }

            if (count == 0)
            {
                return 0;
            }

            return total / count;
        }

        /// <summary>
        /// Calculates the average time in milliseconds that elapsed while this timer was recording.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <returns>Returns the average time in milliseconds that elapsed between each start and stop.</returns>
        /// <remarks>Will return -1 if the timer <see cref="key"/> is not found.</remarks>
        public long AverageMilliseconds(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return -1;
            }

            var model = this.timers[key];
            if (model.Count == 0)
            {
                return 0;
            }

            return model.Timer.ElapsedMilliseconds / model.Count;
        }

        /// <summary>
        /// Calculates the total average time in milliseconds that elapsed while the specified timers were recording.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <returns>Returns the total average time in milliseconds that elapsed between each start and stop for all the specified timers.</returns>
        public long AverageMilliseconds(T[] keys)
        {
            long total = 0;
            var count = 0;
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                total += model.Timer.ElapsedMilliseconds;
                count += model.Count;
            }

            if (count == 0)
            {
                return 0;
            }

            return total / count;
        }

        /// <summary>
        /// Removes the specified timers.
        /// </summary>
        /// <param name="keys">The keys to the timers that will be removed.</param>
        public void Remove(T[] keys)
        {
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                this.timers.Remove(key);
            }
        }

        /// <summary>
        /// Removes a timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        public void Remove(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return;
            }
            this.timers.Remove(key);
        }

        /// <summary>
        /// Resets all the timers.
        /// </summary>
        public void ResetAll()
        {
            foreach (var pair in this.timers)
            {
                pair.Value.Timer.Reset();
            }
        }

        /// <summary>
        /// Resets all the timers.
        /// </summary>
        /// <param name="resetCounts">If true will set each timer start count to 0.</param>
        public void ResetAll(bool resetCounts)
        {
            foreach (var pair in this.timers)
            {
                pair.Value.Timer.Reset();
                if (resetCounts)
                {
                    pair.Value.Count = 0;
                }
            }
        }

        /// <summary>
        /// Gets an array of timer keys.
        /// </summary>
        /// <returns>Returns an array of timer keys.</returns>
        public T[] GetKeys()
        {
            var keys = new T[this.timers.Count];
            this.timers.Keys.CopyTo(keys, 0);
            return keys;
        }

        /// <summary>
        /// Gets the number of created timers.
        /// </summary>
        public int Count
        {
            get
            {
                return this.timers.Count;
            }
        }

        /// <summary>
        /// Sets the enabled state of the timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <param name="enabled">If true the timer will be enabled. If false the timer will be stopped if it is running.</param>
        public void SetEnabled(T key, bool enabled)
        {
            if (!this.timers.ContainsKey(key))
            {
                return;
            }
            var model = this.timers[key];
            model.Enabled = enabled;
        }

        /// <summary>
        /// Sets the enabled state of the specified timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <param name="enabled">If true the timers will be enabled. If false the timers will be stopped if they are running.</param>
        public void SetEnabled(T[] keys, bool enabled)
        {
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                model.Enabled = enabled;
            }
        }

        /// <summary>
        /// Gets the enabled state of the timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        public bool IsEnabled(T key)
        {
            var model = this.timers[key];
            return model.Enabled;
        }

        /// <summary>
        /// Gets the enabled state of the specified timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <param name="enabled">Will contain the enabled state for each specified key.</param>
        public void IsEnabled(T[] keys, out bool[] enabled)
        {
            var enabledStates = new bool[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                var model = this.timers[keys[i]];
                enabledStates[i] = model.Enabled;
            }

            enabled = enabledStates;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <remarks>Will not start if the enabled state is false.</remarks>
        public void Start(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return;
            }

            var model = this.timers[key];
            if (!model.Enabled)
            {
                return;
            }
            model.Count++;
            model.Timer.Start();
        }

        /// <summary>
        /// Starts the timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <remarks>Will not start if the timers enabled state is false.</remarks>
        public void Start(T[] keys)
        {
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }

                var model = this.timers[key];
                if (!model.Enabled)
                {
                    continue;
                }
                model.Count++;
                model.Timer.Start();
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        public void Stop(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return;
            }

            var model = this.timers[key];
            if (!model.Enabled)
            {
                return;
            }
            model.Timer.Stop();
        }

        /// <summary>
        /// Stops the timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        public void Stop(T[] keys)
        {
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }

                var model = this.timers[key];
                if (!model.Enabled)
                {
                    continue;
                }
                model.Timer.Stop();
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        public void Reset(T key)
        {
            this.Reset(key, false);
        }

        /// <summary>
        /// Resets the timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        public void Reset(T[] keys)
        {
            this.Reset(keys, false);
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <param name="resetCount">If true the start count for the timer will be set to 0.</param>
        public void Reset(T key, bool resetCount)
        {
            if (!this.timers.ContainsKey(key))
            {
                return;
            }
            var model = this.timers[key];
            model.Timer.Reset();
            if (resetCount)
            {
                model.Count = 0;
            }
        }

        /// <summary>
        /// Resets the timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <param name="resetCounts">If true the start count for the timers will be set to 0.</param>
        public void Reset(T[] keys, bool resetCounts)
        {
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                model.Timer.Reset();
                if (resetCounts)
                {
                    model.Count = 0;
                }
            }
        }

        /// <summary>
        /// Resets the timer start count to 0.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        public void ResetCount(T key)
        {
            if (!this.timers.ContainsKey(key))
            {
                return;
            }
            var model = this.timers[key];
            model.Count = 0;
        }

        /// <summary>
        /// Resets each timer start count to 0.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        public void ResetCount(T[] keys)
        {
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                model.Count = 0;
            }
        }

        /// <summary>
        /// Restarts the timer.
        /// </summary>
        /// <param name="key">The key to the timer information.</param>
        /// <param name="resetCount">If true the start count for the timer will be set to 0.</param>
        /// <remarks>The timer will be reset then started again.</remarks>
        public void Restart(T key, bool resetCount)
        {
            if (!this.timers.ContainsKey(key))
            {
                return;
            }
            var model = this.timers[key];
            model.Timer.Reset();
            if (resetCount)
            {
                model.Count = 0;
            }

            this.Start(key);
        }

        /// <summary>
        /// Restarts the specified timers.
        /// </summary>
        /// <param name="keys">The keys to the timer information.</param>
        /// <param name="resetCounts">If true the start count for the timers will be set to 0.</param>
        /// <remarks>Each timer will be reset then started again.</remarks>
        public void Restart(T[] keys, bool resetCounts)
        {
            foreach (var key in keys)
            {
                if (!this.timers.ContainsKey(key))
                {
                    continue;
                }
                var model = this.timers[key];
                model.Timer.Reset();
                if (resetCounts)
                {
                    model.Count = 0;
                }
            }

            this.Start(keys);
        }

        /// <summary>
        /// Gets a singleton instance of the <see cref="PerformanceTesting{T}"/> class.
        /// </summary>
        public static PerformanceTesting<T> Instance
        {
            get
            {
                return singleton ?? (singleton = new PerformanceTesting<T>());
            }
        }
    }
}

#endif