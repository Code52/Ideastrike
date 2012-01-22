using System.Collections.Generic;

namespace Ideastrike.Nancy.Models
{
    public interface IImageRepository
    {
        IEnumerable<Image> GetAll();
        Image Get(int id);

        void Add(Image image);
        void Delete(int id);
        
    }
}