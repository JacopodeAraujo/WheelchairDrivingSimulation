using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEditor;

public class Logger
{
    string dir = "";
    string filename = "";
    string exp = "";
    string filepath = "";
    string subjectname = "";
    string ctrlint = "";
    string endoflog = "End of logging";
    string terminatedlog = "Experiment and Logging terminated";
    int logNameCount = 1;

    DateTime exp_starttime;
    DateTime exp_endtime;

    public StreamWriter writer;

    public Logger(string _dir, string _exp, string _subjectname) {
        dir = _dir;
        exp = _exp;
        subjectname = _subjectname;
        filename = _exp + "_" + _subjectname;
        PrepareTextWriter();
    }

    public Logger(string _dir, string _exp, string _subjectname, string _details) {
        dir = _dir;
        exp = _exp;
        subjectname = _subjectname;
        filename = _exp + "_" + _details + "_" + _subjectname;
        PrepareTextWriter();
    }

    public void PrepareTextWriter() {
        filepath = GetFileName();

        while (File.Exists(filepath)) {
            try {
                //Debug.Log("--------E03--------: test");
                if (File.ReadLines(filepath).Last().Equals(endoflog)) {
                    logNameCount++;
                    // Means the .txt has been terminated rightfully and therefore filename for next .txt shall be changed
                    filepath = GetFileName();
                }
                else if (File.ReadLines(filepath).Last().Equals(terminatedlog)) {
                    Debug.Log("--------E01--------: terminatedlog");
                    //File.Move(filepath, "UNFINISHED_" + filepath);
                    break;
                }
                else {
                    Debug.Log("--------E02--------: File without endoflog nor terminatedlog");
                    File.Move(filepath, "Corrupted" + filepath);
                }
            }
            catch (InvalidOperationException ex) { Debug.Log("File is probably empty. Deleting file."); Debug.Log(ex); File.Delete(filepath); break; }
        }
        writer = new StreamWriter(filepath, true);
        //LogHeader();
    }

    public void TerminateLogger() {
        Debug.Log("Logger Terminated");
        if (writer.BaseStream != null) {
            writer.WriteLine(terminatedlog);
            writer.Close();
            int i = 0;
            string alternative_text = "/TERMINATED/" + "T"+i;
            if (File.Exists(GetAlternativeFileName(alternative_text))) {
                while (File.Exists(GetAlternativeFileName(alternative_text))) {
                    i++;
                    alternative_text = "/TERMINATED/" + "T"+i;
                }
                File.Move(filepath, GetAlternativeFileName(alternative_text));
            }
            else File.Move(filepath, GetAlternativeFileName(alternative_text));
        }
    }

    public void LogLine(string text) {
        writer.WriteLine(text);
    }

    public void LogHeader(string _title, string _description, DateTime _starttime) {
        if (writer.BaseStream == null) {
            writer = new StreamWriter(filepath, true);
        }
        exp_starttime = _starttime;
        writer.WriteLine(_title);
        writer.WriteLine("Subject: " + subjectname);
        writer.WriteLine("Date and start time: " + _starttime);
        writer.WriteLine("");
        writer.WriteLine(_description);
        writer.WriteLine("");
        writer.WriteLine("");
        writer.WriteLine("______________________________________________________________________________________");

    }
    public void LogColumnNames(string columnnames) {
        writer.WriteLine("\r\n");
        writer.WriteLine(columnnames);
        writer.WriteLine("-------------------------------------------------------------------------------------");

    }
    public void EndLog() {
        exp_endtime = DateTime.Now;
        writer.WriteLine("");
        writer.WriteLine("End time: " + exp_endtime);
        writer.WriteLine("Total experiment time: " + (exp_endtime - exp_starttime));
        writer.WriteLine(endoflog);
        Debug.Log(endoflog);
        writer.Close();
        //Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(filepath);
        Resources.Load(filepath);
    }
    // Make sure the order of the lists are accordingly the row info of the LogHeader()
    public void WriteListsToLog(List<IList> list, int numOfListRows, string rowtext) {
        if (list != null) {
            for (int i = 0; i <= numOfListRows - 1; i++) {
                writer.Write(rowtext+" "+(i + 1) + ": ");
                for (int j = 0; j <= list.Count-1; j++)
                    writer.Write("\t" + list[j][i].ToString());
                writer.Write(writer.NewLine);
            }
        }

    }

    private string GetFileName() {
        return string.Format("{0}{1}{2}.txt",
                            dir, filename, logNameCount);
    }
    public string GetImageFileName(int _resWidth, int _resHeight) {
        return string.Format("{0}{1}{2}_{3}x{4}.png",
                                dir, filename, logNameCount, _resWidth, _resHeight);
    }
    
    
    private string GetAlternativeFileName(string alternative_text) {
        return string.Format("{0}{1}{2}_{3}.txt",
                         dir, alternative_text, filename, logNameCount);
    }


}
