using System;
using System.Collections.Generic;
using System.IO;
using Gtk;
using System.Drawing;
public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        data = new List<TOSGEMINFO>();
        button15.Sensitive = false;
        button17.Sensitive = false;

        Gdk.Color color = Gdk.Color.Zero;
        Gdk.Color.Parse("#0080FF", ref color);//blue
        button1.ModifyBg(StateType.Normal, color);

        Gdk.Color.Parse("#FF0000", ref color);//fire
        button3.ModifyBg(StateType.Normal, color);

        Gdk.Color.Parse("#00FF00", ref color);//green
        button5.ModifyBg(StateType.Normal, color);

        Gdk.Color.Parse("#FFFF00", ref color);//yellow
        button7.ModifyBg(StateType.Normal, color);

        Gdk.Color.Parse("#9400D3", ref color);//purple
        button11.ModifyBg(StateType.Normal, color);

        Gdk.Color.Parse("#FFC0CB", ref color);//heart
        button2.ModifyBg(StateType.Normal, color);

        ReverseGemTypeButton();
        //prev
        button22.Sensitive = false;
        //next
        button20.Sensitive = false;
    }
    //可能以後會新增新的東西
    enum GEMTYPE{ nocolor = 0, blue, red, green, yellow, purple , heart }
    class TOSGEMINFO{
        public string file_path;
        public GEMTYPE gemtype;
        public string filename;//without ext.
    }
    List<TOSGEMINFO> data;
    int current_pos = 0;
    string gem_directory = "";

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnButton13Clicked(object sender, EventArgs e)
    {
        var isdiscard = AskifDiscardChanges();
        if(!isdiscard){
            return;
        }else{
            ResetButton();
        }

        var dio = new FileChooserDialog(
            "Select Folder",
            this,
            FileChooserAction.SelectFolder,
            "Cancel",
            ResponseType.Cancel,
            "Ok",
            ResponseType.Ok
        ){
            TransientFor = Toplevel as Window,
            WindowPosition = WindowPosition.Center
        };
        try{
            var foo = dio.Run();
            data.Clear();
            if ( foo == (int)ResponseType.Ok){
                Console.WriteLine("Return OK " + dio.Filename);
                string abs_folder = dio.Filename;
                gem_directory = dio.Filename;
                var pngpath = Directory.GetFiles(abs_folder, "bar*.png", SearchOption.TopDirectoryOnly);

                foreach (var item in pngpath){
                    var info = new TOSGEMINFO();
                    info.file_path = item;
                    info.gemtype = GEMTYPE.nocolor;
                    info.filename = System.IO.Path.GetFileNameWithoutExtension(item);
                    data.Add(info);
                }
                LoadFromCSVifExist();
                FirstLoadImage();
            }
        }
        finally{
            dio.Destroy();
        }
    }
    private bool AskifDiscardChanges(){
        bool result = true;
        if( data.Count!= 0){
            var dio = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.YesNo,
                                        "是否放棄變更？\nDiscard Changes?")
            {
                Title = "I am Vengence, I am the Night, I am Batman!",
                TransientFor = Toplevel as Window,
                WindowPosition = WindowPosition.Center
            };
            try{
                var foo = dio.Run();
                if(foo == (int)ResponseType.Yes){
                    result = true;
                }else if (foo == (int)ResponseType.No){
                    result = false;
                }
            }finally{
                dio.Destroy();
            }
        }
        return result;
    }
    private void ReverseGemTypeButton(){
        var reverse = !button1.Sensitive;
        button1.Sensitive = reverse;
        button3.Sensitive = reverse;
        button5.Sensitive = reverse;
        button7.Sensitive = reverse;
        button11.Sensitive = reverse;
        button2.Sensitive = reverse;
    }
    private void ResetButton()
    {
        var foo = false;
        button1.Sensitive = foo;
        button3.Sensitive = foo;
        button5.Sensitive = foo;
        button7.Sensitive = foo;
        button11.Sensitive = foo;
        button2.Sensitive = foo;
        button22.Sensitive = foo;
        button20.Sensitive = foo;
        button15.Sensitive = foo;

        button17.Sensitive = foo;
        Gdk.Color color = Gdk.Color.Zero;
        string color_str = "#FFFFFF";
        Gdk.Color.Parse(color_str, ref color);
        button4.ModifyBg(StateType.Normal, color);
    }
    private void InitialAllButtonSenstivite(){
        ResetButton();
        button20.Sensitive = true;
        button17.Sensitive = true;
        ReverseGemTypeButton();
    }

    private void ChangePos(bool isnextclicked)
    {
        current_pos = current_pos + (isnextclicked ? 1 : (-1));
        if (current_pos < 0)
        {
            current_pos = 0;
        }
        if (current_pos >= data.Count)
        {
            current_pos = data.Count - 1;
        }
        button22.Sensitive = current_pos > 0; 
        button20.Sensitive = current_pos < (data.Count - 1);

        label1.Text = String.Format("第{0}個", current_pos);
        ShowGemTypeOnApp();
        LoadImage();
    }
    private void LoadImage(){
        if (current_pos < data.Count && current_pos >= 0){
            image2.File = data[current_pos].file_path;
            var width = image2.Pixbuf.Width;
            var height = image2.Pixbuf.Height;
            var sqq = image2.WidthRequest > image2.HeightRequest ? image2.WidthRequest : image2.HeightRequest;
            image2.Pixbuf = image2.Pixbuf.ScaleSimple(sqq, sqq, Gdk.InterpType.Bilinear);
        }
    }
    private void ShowGemTypeOnApp()
    {
        Gdk.Color color = Gdk.Color.Zero;
        string color_str = "#FFFFFF";

        if (current_pos < data.Count && current_pos >= 0)
        {
            switch (data[current_pos].gemtype)
            {
                case GEMTYPE.nocolor:
                    break;
                case GEMTYPE.blue:
                    color_str = "#0080FF";//blue
                    break;
                case GEMTYPE.red:
                    color_str = "#FF0000";
                    break;
                case GEMTYPE.green:
                    color_str = "#00FF00";
                    break;
                case GEMTYPE.yellow:
                    color_str = "#FFFF00";
                    break;
                case GEMTYPE.purple:
                    color_str = "#9400D3";
                    break;
                case GEMTYPE.heart:
                    color_str = "#FFC0CB";
                    break;
                default:
                    break;
            }
        }
        Gdk.Color.Parse(color_str, ref color);
        button4.ModifyBg(StateType.Normal, color);
    }
    private void FirstLoadImage(){
        if (data.Count > 0 ){
            current_pos = 0;
            label1.Text = String.Format("第{0}個", current_pos);
            LoadImage();
            button15.Sensitive = true;
            ShowGemTypeOnApp();
        }
    }
    private void DeterminGemType(GEMTYPE gem){
        if (current_pos >= 0 && current_pos < data.Count) {
            data[current_pos].gemtype = gem;
        }
    }
    private void LoadFromCSVifExist(){
        if (String.IsNullOrEmpty(gem_directory)) { return; }
        if (!System.IO.Directory.Exists(gem_directory)) { return; }
        string csv = System.IO.Path.Combine(gem_directory, "gem_data.csv");
        if (!System.IO.File.Exists(csv)) { return; }
        try
        {
            using (var file = System.IO.File.OpenText(csv))
            {
                //bar10,0
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine().Split(',');
                    foreach (var item in data)
                    {
                        try
                        {
                            if (line[0] == item.filename)
                            {
                                item.gemtype = (GEMTYPE)int.Parse(line[1]);
                            }
                        }
                        finally { }
                    }
                }
            }
        }finally{}
    }
    private void SaveToCSV(){
        if(String.IsNullOrEmpty(gem_directory)){
            return;
        }
        if(!Directory.Exists(gem_directory)){
            return;
        }
        string filepath = System.IO.Path.Combine(gem_directory, "gem_data.csv");
        using(var file = File.Create(filepath)){
            foreach (var item in data)
            {
                //var filename = System.IO.Path.GetFileName(item.file_path);
                var result = String.Format("{0},{1}{2}", item.filename, (int)item.gemtype, Environment.NewLine);
                var bdata = System.Text.Encoding.UTF8.GetBytes(result);
                file.Write(bdata, 0, bdata.Length);
            }
        }
    }



    //Hooman Recognize
    protected void OnButton15Clicked(object sender, EventArgs e)
    {
        InitialAllButtonSenstivite();
    }

    //previous
    protected void OnButton22Clicked(object sender, EventArgs e)
    {
        ChangePos(false);
    }

    //next
    protected void OnButton20Clicked(object sender, EventArgs e)
    {
        ChangePos(true);
    }

    //water
    protected void OnButton1Clicked(object sender, EventArgs e)
    {
        DeterminGemType(GEMTYPE.blue);
        ChangePos(true);
    }
    //fire
    protected void OnButton3Clicked(object sender, EventArgs e)
    {
        DeterminGemType(GEMTYPE.red);
        ChangePos(true);
    }
    //leaf
    protected void OnButton5Clicked(object sender, EventArgs e)
    {
        DeterminGemType(GEMTYPE.green);
        ChangePos(true);
    }
    //light
    protected void OnButton7Clicked(object sender, EventArgs e)
    {
        DeterminGemType(GEMTYPE.yellow);
        ChangePos(true);
    }
    //dark
    protected void OnButton11Clicked(object sender, EventArgs e)
    {
        DeterminGemType(GEMTYPE.purple);
        ChangePos(true);
    }

    protected void OnButton17Clicked(object sender, EventArgs e)
    {
        SaveToCSV();
        var dio = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok,
                            "成功")
        {
            Title = "I am Vengence, I am the Night, I am Batman!",
            TransientFor = Toplevel as Window,
            WindowPosition = WindowPosition.Center,
            WidthRequest = 200
        };
        try
        {
            var foo = dio.Run();
        }
        finally
        {
            dio.Destroy();
        }
    }

    protected void OnButton2Clicked(object sender, EventArgs e)
    {
        DeterminGemType(GEMTYPE.heart);
        ChangePos(true);
    }

    protected void OnButton6Clicked(object sender, EventArgs e)
    {
        var filter = new FileFilter();
        filter.AddPattern("*.png");
        var dio = new FileChooserDialog(
            "Select PNG File",
            this,
            FileChooserAction.Open,
            "Cancel",
            ResponseType.Cancel,
            "Ok",
            ResponseType.Ok
        )
        {
            TransientFor = Toplevel as Window,
            WindowPosition = WindowPosition.Center,
            Filter = filter
        };
        try
        {
            var foo = dio.Run();
            data.Clear();
            if (foo == (int)ResponseType.Ok)
            {
                var filepath = dio.Filename;
                var t1 = new System.Threading.Thread(
                    () => {
                    //may have memory path
                    var folder = System.IO.Path.GetDirectoryName(filepath);
                        CutGemAndSave(filepath, folder);
                });
                t1.IsBackground = true;
                t1.Start();
            }
        }
        finally
        {
            dio.Destroy();
        }
    }
    private void CutGemAndSave(string pngpath,string save_folder){
        using (var sourcepng = new Bitmap(pngpath))
        {
            var png = sourcepng.GetThumbnailImage(180, 320, null, IntPtr.Zero);
            int height = png.Size.Height;
            int width = png.Size.Width;

            int padding = (height - (width * 3 / 2)) / 2;
            int target_x = 0;
            int target_y = padding + (width * 2 / 3);
            int target_width = width;
            int target_height = width * 5 / 6;

            var guid = System.Guid.NewGuid();
            var newfolder = System.IO.Path.Combine(save_folder, guid.ToString());

            System.IO.Directory.CreateDirectory(newfolder);
            using (var board = new Bitmap(target_width, (target_height)))
            {
                using (var gp = Graphics.FromImage(board))
                {
                    var foo = new Rectangle(0, 0, target_width, target_height);
                    var bar = new Rectangle(target_x, target_y, target_width, target_height);
                    gp.DrawImage(png, foo, bar, GraphicsUnit.Pixel);
                }
                board.Save(System.IO.Path.Combine(newfolder, "board.png"));

                int target = width / 6;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        int t_x = j * target;
                        int t_y = i * target;
                        var foo = new Rectangle(0, 0, target, target);
                        var bar = new Rectangle(t_x, t_y, target, target);
                        using (var gem = new Bitmap(target, target))
                        {
                            using (var gp = Graphics.FromImage(gem))
                            {
                                gp.DrawImage(board, foo, bar, GraphicsUnit.Pixel);
                            }
                            gem.Save(System.IO.Path.Combine(newfolder, String.Format("bar{0}{1}.png", i, j)));
                        }
                    }
                }
            }
        }
    }

}
