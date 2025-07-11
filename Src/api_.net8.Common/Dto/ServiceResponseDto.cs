using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_.net9.Common.Dto
{
    public class ServiceResponseDto<T>
    {
        public T? Data { get; set; }
        public bool succsess { get; set; } = true;
        public string? Message { get; set; } = "";

    }

}