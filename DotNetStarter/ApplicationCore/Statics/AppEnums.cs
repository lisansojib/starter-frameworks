namespace ApplicationCore.Statics
{
    /// <summary>
    /// Repeat After
    /// </summary>
    public enum RepeatAfterEnum
    {
        /// <summary>
        /// No Repeat
        /// </summary>
        NoRepeat = 1,

        /// <summary>
        /// Repeat Every Year
        /// </summary>
        EveryYear = 2,

        /// <summary>
        /// Every Month
        /// </summary>
        EveryMonth = 3,

        /// <summary>
        /// Every Day
        /// </summary>
        EveryDay = 4
    }

    public enum OrderStatus
    {
        Pending = 1,
        Acknowledge = 2,
        Reject = 3,
        Printing = 4,
        ReadyForDelivery = 5,
        Shipped = 6,
        Delivered = 7,
        All = 8
    }
}
