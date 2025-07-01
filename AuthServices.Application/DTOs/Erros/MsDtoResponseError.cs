using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Application.DTOs.Erros
{
    public class MsDtoResponseError
    {
        public int code { get; set; }
        public string message { get; set; }
        public string traceid { get; set; }
        public IEnumerable<object> errors { get; set; }

        public MsDtoResponseError(int code, string message, string traceId, IEnumerable<string> errors)
        {
            this.code = code;
            this.message = message;
            this.traceid = traceId;
            this.errors = errors.Select(e => new { code, message = e });
        }
    }

}
