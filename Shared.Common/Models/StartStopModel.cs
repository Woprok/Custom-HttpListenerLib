using Shared.Common.Enums;
using Shared.Common.Interfaces;

namespace Shared.Common.Models
{
    /// <inheritdoc cref="IStartStopModel" />
    /// <inheritdoc cref="ThreadModel" />
    public abstract class StartStopModel : ThreadModel, IStartStopModel
    {
        /// <summary>
        /// Current readonly value of model state.
        /// </summary>
        protected ModelState InternalModelState { get; private set; }

        protected StartStopModel()
        {
            InternalModelState = ModelState.New;
        }

        /// <summary>
        /// Default method invoked on successful Initialize.
        /// </summary>
        protected abstract void OnModelInitialize();

        /// <summary>
        /// Default method invoked on successful Start.
        /// </summary>
        protected abstract void OnModelStart();

        /// <summary>
        /// Default method invoked on successful Stop.
        /// </summary>
        protected abstract void OnModelStop();

        /// <inheritdoc/>
        /// <exception cref="Shared.Common.Exceptions.InvalidCallException"/>
        /// <exception cref="Shared.Common.Exceptions.UnknownEnumValueException"/>
        public void Initialize()
        {
            lock (CriticalAccessLock)
            {
                switch (InternalModelState)
                {
                    case ModelState.New:
                        InternalModelState = ModelState.Initialized;
                        OnModelInitialize();
                        break;
                    case ModelState.Initialized:
                    case ModelState.Started:
                    case ModelState.Stopped:
                        throw new InvalidCallException(nameof(Initialize) + InternalModelState);
                    default:
                        throw new UnknownEnumValueException(nameof(ModelState));
                }
            }
        }

        /// <inheritdoc/>
        /// <exception cref="Shared.Common.Exceptions.InvalidCallException"/>
        /// <exception cref="Shared.Common.Exceptions.UnknownEnumValueException"/>
        public void Start()
        {
            lock (CriticalAccessLock)
            {
                switch (InternalModelState)
                {
                    case ModelState.Initialized:
                        CreateToken();
                        InternalModelState = ModelState.Started;
                        OnModelStart();
                        break;
                    case ModelState.New:
                    case ModelState.Started:
                    case ModelState.Stopped:
                        throw new InvalidCallException(nameof(Start) + InternalModelState);
                    default:
                        throw new UnknownEnumValueException(nameof(ModelState));
                }
            }
        }

        /// <inheritdoc/>
        /// <exception cref="Shared.Common.Exceptions.InvalidCallException"/>
        /// <exception cref="Shared.Common.Exceptions.UnknownEnumValueException"/>
        public void Stop()
        {
            lock (CriticalAccessLock)
            {
                switch (InternalModelState)
                {
                    case ModelState.Started:
                        CancelToken();
                        InternalModelState = ModelState.Stopped;
                        OnModelStop();
                        break;
                    case ModelState.New:
                    case ModelState.Initialized:
                    case ModelState.Stopped:
                        throw new InvalidCallException(nameof(Initialize) + InternalModelState);
                    default:
                        throw new UnknownEnumValueException(nameof(ModelState));
                }
            }
        }
    }
}