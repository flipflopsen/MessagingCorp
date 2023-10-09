using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Common.HttpStuff
{
    /* 
     * All Base64ed -> Encrypted with AES or sth
     * -----
     * UserId == 123;
     * Challenge == abcdefg;
     * Action == SomeAction;
     * AdditionalData == SomeBase64;
     * -----
     * Base64AdditionalData == Some;Additional;Data;2134;313;sth;HasToEndWith:Challenge:::SomeMessageCorpConstant:::Challenge
     */
    public class CorpPostRequestFormat
    {
        public string UserId { get; set; }
        public string Challenge { get; set; }
        public string Action { get; set; }
        public string AdditionalData { get; set; }
    }
}
