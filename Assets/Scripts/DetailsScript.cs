

public static class DetailsScript : object 
{
    private static int[] exp3_levels = new int[] { 2 };
    private static string subjectname = "NONAME";
    private static string exp = "EXP";
    private static string ctrlint = "NONE";
    private static int MIEMOlevels = 3;
    private static int MIEMOtrials = 3;
    private static int[] MIEMOincorrects = new int[] { 3, 2, 1 };

    private static string Lcomb1 = "";
    private static string Lcomb2 = "";
    private static string Lcomb3 = "";
    private static int repeat=0;

    public static string GetName() {
        return subjectname;
    }

    public static string GetControl() {
        return ctrlint;
    }

    public static void SetName(string _name) {
        if (_name.Contains(" "))
            _name.Replace(" ", "_");
        subjectname = _name;
    }

    public static void SetControl(string _ctrl) {
        if (_ctrl.Contains(" "))
            _ctrl.Replace(" ", "_");
        ctrlint = _ctrl;
    }

    public static void SetLcomb1(string c) {
        Lcomb1 = c;
    }

    public static string GetLcomb1() {
        if (Lcomb1 != "")
            return Lcomb1;
        else
            return "A";
    }
    public static void SetLcomb2(string c) {
        Lcomb2 = c;
    }

    public static string GetLcomb2() {
        if (Lcomb2 != "")
            return Lcomb2;
        else
            return "A";
    }
    public static void SetLcomb3(string c) {
        Lcomb3 = c;
    }

    public static string GetLcomb3() {
        if (Lcomb3 != "")
            return Lcomb3;
        else
            return "A";
    }

    public static void SetMIEMOlevels(int _levels) {
        MIEMOlevels = _levels;
    }

    public static int GetMIEMOlevels() {
        return MIEMOlevels;
    }
    public static int GetMIEMOtrials() {
        return MIEMOtrials;
    }
    public static void SetMIEMOtrials(int _trials) {
        MIEMOtrials = _trials;
    }

    public static int[] GetMIEMOincorrects() {
        return MIEMOincorrects;
    }
    public static void SetMIEMOincorrects(int[] _trials) {
        MIEMOincorrects = _trials;
    }

    public static int[] GetExp3Levels() {
        return exp3_levels;
    }
    public static void SetExp3Levels(int[] _exp3_levels) {
        exp3_levels = _exp3_levels;
    }

    //public static int GetStartLevelsRepeat() {
    //    return repeat;
    //}

    //public static void SetStartLevelsRepeat(int r) {
    //    repeat = r;
    //}
}
