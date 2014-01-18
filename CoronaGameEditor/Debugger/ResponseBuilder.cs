using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Krea.GameEditor.Debugger;

namespace Krea.Debugger
{
    public class ResponseBuilder
    {
        //////////////////////////////////////////
        /// PROPERTIES
        //////////////////////////////////////////
        private static ResponseBuilder instance;
       
        private String MessageToAnalyse;
        
        public List<Backtrace> Backtraces;
        public List<BreakPoint> BreakPoints;
        public List<Watch> Watchs;
        public List<Local> Locals;

        public Int32 CurrentBreakPointLine;
        public String CurrentBreakPointFile;

        public String LastCommandReceive;
        public String CurrentCommandReceive;
        public String CurrentCommandSend;
        public String PreviousAnalysedMessage;
        public Boolean ErrorFound;
        public String ErrorDescription;


        //////////////////////////////////////////
        /// CONSTRUCTORS
        //////////////////////////////////////////
        public static ResponseBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResponseBuilder();
                }
                return instance;
            }
        }
      
        private ResponseBuilder() { }

        //////////////////////////////////////////
        /// INIT METHODS
        //////////////////////////////////////////
        public void Init(String _MessageToAnalyse)
        {
            this.Locals = new List<Local>();
            this.Backtraces = new List<Backtrace>();
            this.MessageToAnalyse = _MessageToAnalyse;
            this.Watchs = new List<Watch>();
            this.BreakPoints = new List<BreakPoint>();
            this.ErrorFound = false;

        }

        //////////////////////////////////////////
        /// UTILITIES METHODS
        //////////////////////////////////////////
        public void syncWithDebugger(List<BreakPoint> _BreakPoints, List<Watch> _Watchs, String LastCommandReceive, String CurCommandReiceive, String CurCommandSend)
        {
            this.BreakPoints = _BreakPoints;
            this.Watchs = _Watchs;
            this.CurrentCommandReceive = CurCommandReiceive;
            this.CurrentCommandSend = CurCommandSend;
            this.LastCommandReceive = LastCommandReceive;
        }
        
        private bool IS_200_OK_VALUE(string text){
            if (text == null) return false;
            if (text.Equals("")) return false;
            if (Regex.IsMatch(text, "^200 OK$"))
            {
                return true;
            }
            return false;
        }

        private bool IS_200_OK_BYTE_VALUE(string text) {
            if (text == null) return false;
            if (text.Equals("")) return false;

            if (Regex.IsMatch(text, "^200 OK(\\s?\\d+)$"))
            {
                return true;
            }
            return false;
        }

        private bool IS_ERROR_401_VALUE(string text)
        {
            if (text == null) return false;
            if (text.Equals("")) return false;
            if (Regex.IsMatch(text, "^200 OK$"))
            {
                return true;
            }
            return false;
        }

        private bool is_backtrace(string text) {
            if (text == null) return false;
            if (text.Equals("")) return false;
            if (text.Equals("__KREA_CHECK_ROW_DONE__")) return false;
            if (Regex.IsMatch(text, @"^(\0)+$"))
            {
                return false;
            }
            if (Regex.IsMatch(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\W{1}\w+\W{1})\s+([0-9a-zA-Z.\s\\_]+):(\d+)"))
            {
                // Match *[ {NUM}] {(KIND)} {At_FILE}:{LineNumber} && [ {NUM}] {KIND} {At_FILE}:{LineNumber}
                return true;
            }
            else if (Regex.IsMatch(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\W{1}\w+\W{1})"))
            {
            // Match [ {NUM}] (Runtime)
                return true;
            }
            else if (Regex.IsMatch(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\W*\w+)"))
            {
                // Match [ {NUM}] VAR
                return true;
            }
              
            else if (Regex.IsMatch(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\w+)\s+([0-9a-zA-Z.\s\\_]+):(\d+)"))
            {
                // Match *[ {NUM}] {KIND} {At_FILE}:{LineNumber} && [ {NUM}] {KIND} {At_FILE}:{LineNumber}
                return true;
            }
            else if (Regex.IsMatch(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\w+)\s+at\s+([0-9a-zA-Z.\s\\_?]+):(\d+)"))
            {
                return true;
            }
            else if (Regex.IsMatch(text, @"\s+\W\s+(\d+)\W\s+(\W)\s+at\s+(\W):(\d+)(200\s+OK\s+)*(\d+)*"))
            {
                return true;
            }
            return false; 
        }

        private bool is_local_vars(string text)
        {
            if (text == null) return false;
            if (text.Equals("")) return false;
            if (text.Equals("__KREA_CHECK_ROW_DONE__")) return false;
            if (Regex.IsMatch(text, @"^(\0)+$"))
            {
                return false;
            }

            if(Regex.IsMatch(text,@"(\\0+)")) return false;

            if (Regex.IsMatch(text, @"([0-9a-zA-Z]+)\s+([^0-9a-zA-Z])\s+([0-9a-zA-Z]+[^0-9a-zA-Z]+)\s+([0-9a-fA-F]{8})(200\s+OK\s+\d+)*"))
            { 
                //Match {Var} = {FuncName:} {TableValue}
                return true; 
            }
            else if (Regex.IsMatch(text, @"([0-9a-zA-Z@-]+)\s*([^0-9a-zA-Z])\s*([0-9a-zA-Z.\s\\_@-]+)(200\s+OK\s+\d+)*"))
            {
                //Match {Var} = {Value};
                return true;
            }
            else if (Regex.IsMatch(text, @"([0-9a-zA-Z]+)\s+([^0-9a-zA-Z])\s+([{]{1})"))
            {
                // Match {VARTab} = {
                return true;
            }
            else if (Regex.IsMatch(text, @"([0-9a-zA-Z\u0022\\]+):([0-9a-zA-Z\u0022\\]+)(200\s+OK\s+\d+)*"))
            {
                //Match {"Tabkey"} = {"TabValue"}
                return true;
            }
            return false;
        }

        private void construct_local(string text) {
            if (text == null) return;
            if (text.Equals("")) return;
        }

        private void construct_backtrace(string text) {
            if (text == null) return;
            if (text.Equals("")) return;
        }

        public void Analyse()
        {
            if (MessageToAnalyse.Equals("")) return;
            List<String> MessageSplited = new List<String>(MessageToAnalyse.Split('\n'));
            PreviousAnalysedMessage = "";
            List<string> localsList = new List<String>();
            List<string> backtracesList = new List<String>();
            int parcourLenght=0;
            for (int i = 0; i < MessageSplited.Count; i++)
            {
                String currentMessage = MessageSplited[i];
                List<String> desasembledMessage =  new List<String>(currentMessage.Split(' '));

                if (Regex.IsMatch(currentMessage, @"^(\0)+$"))
                {
                    MessageSplited[i] = "__KREA_CHECK_ROW_DONE__";
                    continue;
                }
                if(currentMessage.IndexOf("\0") == 0){
                    MessageSplited[i] = "__KREA_CHECK_ROW_DONE__";
                    continue;
                }
                if (IS_200_OK_VALUE(currentMessage)) { 
                    //200 OK
                    //Check to Done.
                    PreviousAnalysedMessage = currentMessage;
                    MessageSplited[i] = "__KREA_CHECK_ROW_DONE__";
                }
                else if (IS_200_OK_BYTE_VALUE(currentMessage)) {
                    if (currentMessage.Equals("200 OK 0")) {
                        //200 OK 0
                        //Check to Done
                        PreviousAnalysedMessage = currentMessage;
                        MessageSplited[i] = "__KREA_CHECK_ROW_DONE__";
                    }
                    else
                    {
                        //200 OK XX 
                        //Do nothing, next loops, we get data!
                        PreviousAnalysedMessage = currentMessage;
                        continue;
                    }

                }
                else if (is_backtrace(currentMessage)) {
                    //Check for data arrival
                    if (IS_200_OK_BYTE_VALUE(PreviousAnalysedMessage))
                    {
                        int byteCount = Convert.ToInt32(PreviousAnalysedMessage.Substring(6, PreviousAnalysedMessage.Length - 6));

                        if (currentMessage.Length == byteCount && parcourLenght == 0)
                        {
                            Backtraces.Add(registerBacktrace(currentMessage.Substring(0, byteCount)));
                            parcourLenght = 0;
                            PreviousAnalysedMessage = "__KREA_CHECK_ROW_DONE__";

                        }
                        else if (currentMessage.Length < byteCount && (parcourLenght + currentMessage.Length) <= byteCount)
                        {
                            Backtraces.Add(registerBacktrace(currentMessage.Substring(0, currentMessage.Length)));
                            parcourLenght += currentMessage.Length;

                        }
                        else if (currentMessage.Length < byteCount && (parcourLenght + currentMessage.Length) > byteCount)
                        {
                            int parcourRest = byteCount - parcourLenght-1;
                            Backtraces.Add(registerBacktrace(currentMessage.Substring(0, currentMessage.Length - (currentMessage.Length - parcourRest))));
                            PreviousAnalysedMessage = currentMessage.Substring(currentMessage.Length - (currentMessage.Length - parcourRest) - 1, currentMessage.Length - (currentMessage.Length - (currentMessage.Length - parcourRest) - 1));
                            parcourLenght = 0;

                        }
                        else if (currentMessage.Length > byteCount)
                        {
                            int parcourRest = byteCount - parcourLenght;
                            Backtraces.Add(registerBacktrace(currentMessage.Substring(0, byteCount)));
                            PreviousAnalysedMessage = "__KREA_CHECK_ROW_DONE__";
                            parcourLenght = 0;
                        }

                        if (parcourLenght >= byteCount)
                        {
                            parcourLenght = 0;
                        }
                    }
                    //backtraces
                }
                else if (is_local_vars(currentMessage)) {
                    if (IS_200_OK_BYTE_VALUE(PreviousAnalysedMessage))
                    {
                        if (currentMessage.IndexOf("\0") == 0)
                            continue;

                        Locals.Add(registerLocal(currentMessage,MessageSplited,i));
                  
                       /* int byteCount = Convert.ToInt32(PreviousAnalysedMessage.Substring(6, PreviousAnalysedMessage.Length - 6));

                        if (currentMessage.Length == byteCount && parcourLenght==0)
                        {
                            localsList.Add(currentMessage.Substring(0, byteCount));
                            parcourLenght = 0;
                            PreviousAnalysedMessage = "__KREA_CHECK_ROW_DONE__";

                        }
                        else if (currentMessage.Length < byteCount && (parcourLenght + currentMessage.Length) <= byteCount)
                        {
                            localsList.Add(currentMessage.Substring(0, currentMessage.Length));
                            parcourLenght += currentMessage.Length;
                        }
                        else if (currentMessage.Length < byteCount && (parcourLenght+currentMessage.Length) > byteCount){
                            int parcourRest = byteCount - parcourLenght-1;
                            localsList.Add(currentMessage.Substring(0, currentMessage.Length - (currentMessage.Length - parcourRest)));
                            PreviousAnalysedMessage = currentMessage.Substring(currentMessage.Length - (currentMessage.Length - parcourRest), currentMessage.Length - (currentMessage.Length - (currentMessage.Length - parcourRest)));
                            parcourLenght =0;

                        }
                        else if (currentMessage.Length > byteCount) {
                            localsList.Add(currentMessage.Substring(0, byteCount));
                            PreviousAnalysedMessage = "__KREA_CHECK_ROW_DONE__";
                            parcourLenght = 0;
                        }

                        if (parcourLenght >= byteCount) {
                            parcourLenght = 0;
                        }*/
                    }
                    else if (Regex.IsMatch(currentMessage, @"([0-9a-zA-Z]+)\s*([^0-9a-zA-Z])\s*([0-9a-zA-Z.\s\\_]+)(200\s+OK\s+\d+)*"))
                    {
                        //Handle Dump
                        for(int j=i; j<MessageSplited.Count;j++){
                            if (j == i) { continue; }
                            if (MessageSplited[j].Equals("{")) { continue; }
                            if (MessageSplited[j].Equals("}")) { continue; }



                            if (MessageSplited[j].IndexOf("\0") == 0)
                            {
                                MessageSplited[j] = "__KREA_CHECK_ROW_DONE__";
                                continue;
                            }
                                

                            Locals.Add(registerLocal(MessageSplited[j], MessageSplited, i));
                            MessageSplited[j] = "__KREA_CHECK_ROW_DONE__";
                        }
                    }
                    //locals
                }

                
            }

        }

        public Backtrace registerBacktrace(string text)
        {
            Backtrace result = new Backtrace();
            Match matchresult = Regex.Match(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\W{1}\w+\W{1})\s+([0-9a-zA-Z.\s\\_]+):(\d+)");
            if (matchresult.Success)
            {
                result.Number = Convert.ToInt32(matchresult.Groups[1].Value);
                result.Kind = matchresult.Groups[2].Value;
                result.File = matchresult.Groups[3].Value;
                result.Line = Convert.ToInt32(matchresult.Groups[4].Value);
            }
            else
            {
                matchresult = Regex.Match(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\W*\w+\W*)");
                if (matchresult.Success)
                {
                    result.Number = Convert.ToInt32(matchresult.Groups[1].Value);
                    result.Kind = matchresult.Groups[2].Value;
                    result.File = "?";
                    result.Line = 0;
                }
                else {
                    matchresult = Regex.Match(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\w+)\s+([0-9a-zA-Z.\s\\_?]+):(\d+)");
                    if (matchresult.Success)
                    {
                        result.Number = Convert.ToInt32(matchresult.Groups[1].Value);
                        result.Kind = matchresult.Groups[2].Value;
                        result.File = matchresult.Groups[3].Value;
                        result.Line = Convert.ToInt32(matchresult.Groups[4].Value);
                    }
                    else {
                        matchresult = Regex.Match(text, @"\W{1,2}\s+(\d+)\W{1}\s+(\w+)\s+([0-9a-zA-Z.\s\\_?]+)");
                        if (matchresult.Success)
                        {
                            result.Number = Convert.ToInt32(matchresult.Groups[1].Value);
                            result.Kind = matchresult.Groups[2].Value;
                            result.File = matchresult.Groups[3].Value;
                            result.Line = -1;
                        }
                        else
                        {
                            matchresult = Regex.Match(text, @"\s+\W\s+(\d+)\W\s+(\W)\s+at\s+(\W):(\d+)(200\s+OK\s+)*(\d+)*");
                            if (matchresult.Success)
                            {
                                try
                                {
                                    result.Number = Convert.ToInt32(matchresult.Groups[1].Value);
                                    result.Kind = matchresult.Groups[2].Value;
                                    result.File = matchresult.Groups[3].Value;
                                    if (matchresult.Groups[5].Value.Contains("OK"))
                                    {
                                        PreviousAnalysedMessage = matchresult.Groups[5].Value + matchresult.Groups[6].Value;
                                    }
                                    result.Line = Convert.ToInt32(matchresult.Groups[4].Value);
                                }
                                catch
                                {

                                }
                            }
                            else {
                                matchresult = Regex.Match(text, @"^\W{1,2}\s+(\d+)\W{1}\s+(\w+)(200\s+OK\s+)*(\d+)*$");
                                if (matchresult.Success)
                                {
                                    try
                                    {
                                        result.Number = Convert.ToInt32(matchresult.Groups[1].Value);
                                        result.Kind = matchresult.Groups[2].Value;
                                        result.File = "?";
                                        if (matchresult.Groups[3].Value.Contains("OK"))
                                        {
                                            PreviousAnalysedMessage = matchresult.Groups[3].Value + matchresult.Groups[4].Value;
                                        }
                                        result.Line = 0;
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            if (result.File.Equals(""))
            {
                string s = "";
            }
            return result;
        }
        public Local registerLocal(string text, List<string> fullmessage, int index) {
            Local result = new Local();
            
            Match matchresult = Regex.Match(text, @"([0-9a-zA-Z]+)\s+([^0-9a-zA-Z])\s+([0-9a-zA-Z]+[^0-9a-zA-Z]+)\s+([0-9a-fA-F]{8})(200\s+OK\s+)*(\d+)*");
            if (matchresult.Success)
            {
                result.Name = matchresult.Groups[1].Value;
                result.Operator = matchresult.Groups[2].Value;
                result.Type = matchresult.Groups[3].Value;
                if (matchresult.Groups[3].Value.Contains("table"))
                {
                    result.Type = "TABLE";
                }
                else if (matchresult.Groups[3].Value.Contains("function"))
                {
                    result.Type = "FUNCTION";
                }
                result.Value = matchresult.Groups[4].Value;

                if (matchresult.Groups[5].Value.Contains("OK")) {
                    this.PreviousAnalysedMessage = matchresult.Groups[5].Value + matchresult.Groups[6].Value;
                }
                
            }
            else {
                matchresult = Regex.Match(text, @"([()0-9a-zA-Z]+[^0-9a-zA-Z][0-9a-zA-Z.:\\_\s()]+)\s+(=)(\s+OK\s+)*(\d+)*(\s+[0-9a-zA-Z]+)*");
                if (matchresult.Success)
                {
                    result.Name = matchresult.Groups[1].Value;
                    result.Operator = matchresult.Groups[2].Value;
                    if (matchresult.Groups[5].Value.Equals(""))
                    {
                        for (int i = index; i < fullmessage.Count; i++)
                        {

                            if (i == index)
                            {

                            }
                            else
                            {
                                result.Value += fullmessage[i];
                            }

                            if (fullmessage[i].Contains("}"))
                            {
                                result.Value += fullmessage[i];
                                fullmessage[i] = "__KREA_CHECK_ROW_DONE__";
                                break;
                            }
                            fullmessage[i] = "__KREA_CHECK_ROW_DONE__";
                        }
         
                    }
                    else {
                        result.Value = matchresult.Groups[5].Value;
                    }
                    result.Type = "USERDATA";
                }
                else
                {
                    matchresult = Regex.Match(text, @"([0-9a-zA-Z\u0022\\@-]+)\s+(=)\s+([0-9a-zA-Z\u0022\\:_.@-]+)(\s+OK\s+\d+)*");
                    if (matchresult.Success)
                    {
                        result.Name = matchresult.Groups[1].Value;
                        result.Operator = matchresult.Groups[2].Value;
                        if (matchresult.Groups[4].Value.Contains("OK"))
                        {
                            PreviousAnalysedMessage = 200 + matchresult.Groups[4].Value;
                            result.Value = matchresult.Groups[3].Value.Substring(0, matchresult.Groups[3].Value.Length - 3);
                        }
                        else
                        {
                            result.Value = matchresult.Groups[3].Value;
                        }
                        result.Type = "FIELD";
                    }
                    else
                    {
                        matchresult = Regex.Match(text, @"([0-9a-zA-Z\u0022\\]+):([0-9a-zA-Z\u0022\\]+)(200\s+OK\s+\d+)*");
                        if (matchresult.Success)
                        {
                            result.Name = matchresult.Groups[1].Value;
                            result.Operator = ":";
                            if (matchresult.Groups[3].Value.Contains("OK"))
                            {
                                result.Value = matchresult.Groups[2].Value.Substring(0, matchresult.Groups[2].Value.Length - 3);
                            }
                            else
                            {
                                result.Value = matchresult.Groups[2].Value;
                            }
                            result.Type = "FIELD";
                        }
                        else
                        {
                            matchresult = Regex.Match(text, @"([0-9a-zA-Z]+)\s+([^0-9a-zA-Z])\s+([{]{1})");
                            if (matchresult.Success)
                            {
                                result.Name = matchresult.Groups[1].Value;
                                result.Operator = matchresult.Groups[2].Value;
                                result.Value = "";
                                for (int i = index; i < fullmessage.Count; i++)
                                {

                                    if (i == index)
                                    {
                                        result.Value += fullmessage[i];
                                    }
                                    else
                                    {
                                        result.Value += fullmessage[i];
                                    }

                                    if (fullmessage[i].Contains("}"))
                                    {
                                        result.Value += fullmessage[i];
                                        fullmessage[i] = "__KREA_CHECK_ROW_DONE__";
                                        break;
                                    }
                                    fullmessage[i] = "__KREA_CHECK_ROW_DONE__";
                                }
                                result.Type = "USERDATA";
                            }
                            else
                            {
                                matchresult = Regex.Match(text, @"([0-9a-zA-Z]+)\s+([^0-9a-zA-Z])\s+([0-9a-zA-Z]+)(200\s+OK\s+)(\d+)");
                                if (matchresult.Success)
                                {
                                    result.Name = matchresult.Groups[1].Value;
                                    result.Operator = matchresult.Groups[2].Value;
                                    result.Value = matchresult.Groups[3].Value;
                                    if (matchresult.Groups[4].Value.Contains("OK"))
                                    {
                                        PreviousAnalysedMessage = matchresult.Groups[4].Value + matchresult.Groups[5].Value;
                                    }
                                    result.Type = "FIELD";
                                }
                            }

                        }
                    }    
                }
                
            }
            //if (result.Name.Equals("")) {
            //    string s = "";
            //}
            return result;
        }
        public String ToString()
        {
            String StateResult = "";
            if (this.Locals != null)
            {
                if (this.Locals.Count > 0)
                {
                    StateResult += "Register Locals :\n";
                    for (int i = 0; i < this.Locals.Count; i++)
                    {
                        StateResult += "[" + this.Locals[i].Type + "]" + this.Locals[i].Name + " = " + this.Locals[i].Value + ";\n";
                    }

                }
            }

            if (this.Watchs != null)
            {
                if (this.Watchs.Count > 0)
                {
                    StateResult += "Register Watch :\n";
                    for (int i = 0; i < this.Watchs.Count; i++)
                    {
                        StateResult += "Watch n°" + this.Watchs[i].Number + " with expression : " + this.Watchs[i].Expression + "\n";
                    }

                }
            }

            if (this.Backtraces != null)
            {
                if (this.Backtraces.Count > 0)
                {
                    StateResult += "Register Backtraces :\n";
                    for (int i = 0; i < this.Backtraces.Count; i++)
                    {
                        StateResult += "[" + this.Backtraces[i].Number + "] " + this.Backtraces[i].Kind + " : " + this.Backtraces[i].File + " " + this.Backtraces[i].Line + "\n";
                    }

                }
            }
           
            
            if (this.BreakPoints.Count > 0)
            {
                StateResult += "Register BreakPoints :\n";
                for (int i = 0; i < this.BreakPoints.Count; i++)
                {
                    StateResult += "" + this.BreakPoints[i].File + " on line " + this.BreakPoints[i].LineNumber + "\n";
                }

            }
            
            StateResult += "\n";
            if (this.CurrentBreakPointFile != null && !this.CurrentBreakPointFile.Equals("")) {
                StateResult += "> Current BreakPoint File : "+this.CurrentBreakPointFile+"\n";
            }
            if (this.CurrentBreakPointLine > -1)
            {
                StateResult += "> Current Line in File : " + this.CurrentBreakPointLine.ToString() + "\n";
            }
            if (this.ErrorFound == true) {
                StateResult += "\n[Error Info]\n";
                StateResult += this.ErrorDescription + "\n";

            }
            return StateResult;
        }
    }
}
