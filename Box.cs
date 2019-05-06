using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraOefeningenModule2
{
    class Box
    {
        private string text="";
        private bool isEmpty;

        public void InputText(string inputText)
        {
            text = inputText;
        }

        public string OutputText()
        {
            return text;
        }

        public void Shake()
        {
            Random shaker = new Random();
            int shake = shaker.Next(0, 2);
            if (shake == 1)
            {
                text = "";
                
            }
        }
        public bool IsEmpty
        {
            get
            {
                if (text == "")
                    isEmpty = true;
                else
                    isEmpty = false;
                return isEmpty;
            }
        }
    }
}
