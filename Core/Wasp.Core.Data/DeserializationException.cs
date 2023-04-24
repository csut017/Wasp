using System.Runtime.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// An exception from the deserialization process.
    /// </summary>
    [Serializable]
    internal class DeserializationException
        : Exception
    {
        public DeserializationException()
        {
        }

        public DeserializationException(string? message, int? lineNumber = null, int? linePosition = null) : base(message)
        {
            this.LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        public DeserializationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DeserializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            LineNumber = GetNullableInt(info, nameof(LineNumber));
            LinePosition = GetNullableInt(info, nameof(LinePosition));
        }

        public int? LineNumber { get; private set; }

        public int? LinePosition { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(LineNumber), LineNumber ?? -1, typeof(int));
            info.AddValue(nameof(LinePosition), LinePosition ?? -1, typeof(int));
        }

        private int? GetNullableInt(SerializationInfo info, string name)
        {
            var value = info.GetInt32(name);
            return value < 0 ? null : value;
        }
    }
}