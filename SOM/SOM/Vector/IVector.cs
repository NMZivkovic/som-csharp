using System.Collections.Generic;

namespace SOM.VectorNamespace
{
    public interface IVector : IList<double>
    {
        double EuclidianDistance(IVector vector);
    }
}
