using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectiveMemory.Core.ResultModels
{
    public class ResultModel<T> : BaseResult
    {
        public T? Data { get; set; }

        public static ResultModel<T> Ok(T data)
            => new ResultModel<T> { IsSuccess = true, Data = data };

        public static ResultModel<T> Fail(string error)
            => new ResultModel<T> { IsSuccess = false, ErrorMessage = error };
    }
}
