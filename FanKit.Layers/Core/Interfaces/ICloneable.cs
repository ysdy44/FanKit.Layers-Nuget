namespace FanKit.Layers.Core
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='ICloneable']/*" />
    public interface ICloneable<T>
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='ICloneable.Clone']/*" />
        T Clone();

        /// <include file="doc/docs.xml" path="docs/doc[@for='ICloneable.CloneWithDepth']/*" />
        T Clone(int depth);
    }
}