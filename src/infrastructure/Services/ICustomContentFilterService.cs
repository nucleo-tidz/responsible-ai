namespace infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICustomContentFilterService
    {
        Task<string> AnalyzeCustomCategoryAsync(string text, string categoryName);
    }
}
