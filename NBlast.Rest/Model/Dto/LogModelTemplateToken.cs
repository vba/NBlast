namespace NBlast.Rest.Model.Dto
{
    public class LogModelTemplateToken
    {
        public string PropertyName { get; set; }
        public string Text { get; set; }

        protected bool Equals(LogModelTemplateToken other)
        {
            return string.Equals(PropertyName, other.PropertyName) && string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((LogModelTemplateToken) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((PropertyName?.GetHashCode() ?? 0) * 397) ^ (Text?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(LogModelTemplateToken left, LogModelTemplateToken right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LogModelTemplateToken left, LogModelTemplateToken right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"PropertyName: {PropertyName}, Text: {Text}";
        }
    }
}