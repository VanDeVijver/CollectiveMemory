using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectiveMemory.Core.ResultModels
{
    public abstract class BaseResult
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }
        public List<string> Errors { get; protected set; } = new();

        public static TResult Ok<TResult>() where TResult : BaseResult, new()
            => new TResult { IsSuccess = true };

        public static TResult Fail<TResult>(string error) where TResult : BaseResult, new()
            => new TResult { IsSuccess = false, ErrorMessage = error };
    }
}
