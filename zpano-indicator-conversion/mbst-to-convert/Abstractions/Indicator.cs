using System.Runtime.Serialization;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// An abstract indicator.
    /// </summary>
    [DataContract]
    public abstract class Indicator : IIndicator
    {
        #region Members and accessors
        /// <summary>
        /// Identifies the indicator.
        /// </summary>
        [DataMember]
        protected string name;

        /// <summary>
        /// Identifies the indicator.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Identifies an instance of the indicator.
        /// </summary>
        [DataMember]
        protected string moniker;

        /// <summary>
        /// Identifies an instance of the indicator.
        /// </summary>
        public string Moniker => moniker;

        /// <summary>
        /// Describes the indicator.
        /// </summary>
        [DataMember]
        protected string description;

        /// <summary>
        /// Describes the indicator.
        /// </summary>
        public string Description => description;

        /// <summary>
        /// Is the indicator primed.
        /// </summary>
        [DataMember]
        protected bool primed;

        /// <summary>
        /// Is the indicator primed.
        /// </summary>
        public bool IsPrimed { get { lock (updateLock) { return primed; } } }

        /// <summary>
        /// The Ohlcv component to use.
        /// </summary>
        [DataMember]
        protected OhlcvComponent ohlcvComponent;
        /// <summary>
        /// The Ohlcv component to use.
        /// </summary>
        public OhlcvComponent OhlcvComponent
        {
            get { return ohlcvComponent; }
            set { ohlcvComponent = value; }
        }

        /// <summary>
        /// The lock object.
        /// </summary>
        [DataMember]
        protected readonly object updateLock = new object();
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new object.
        /// </summary>
        /// <param name="name">Identifies the indicator.</param>
        /// <param name="description">Describes the indicator.</param>
        /// <param name="ohlcvComponent">Ohlcv component.</param>
        protected Indicator(string name, string description, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
        {
            this.name = name;
            this.description = description;
            this.ohlcvComponent = ohlcvComponent;
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets the indicator.
        /// </summary>
        public abstract void Reset();
        #endregion
    }
}
