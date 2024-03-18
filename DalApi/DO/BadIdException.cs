using System;

namespace DO
{
    [Serializable]
    public class BadIdException : Exception
    {
        public int? ID;
        public BadIdException(int? id) : base() => ID = id;

        public BadIdException(int? id, string message) : base(message) => ID = id;

        public BadIdException(int? id, string message, Exception innerException)
            : base(message, innerException) => ID = id;

        public override string ToString()
        {
            return base.ToString() + $",bad id:{ID}";
        }
    }
    [Serializable]
    public class XmlFileLoadCreateException : Exception
    {
        

        public string filePath;
        public XmlFileLoadCreateException(string fp) : base() => filePath = fp;

        public XmlFileLoadCreateException(string fp, string message) : base(message) => filePath = fp;

        public XmlFileLoadCreateException(string fp, string message, Exception innerException)
            : base(message, innerException) => filePath = fp;

        public override string ToString()
        {
            return base.ToString() + $"cant open:{filePath}";
        }

    }


}
