using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using System.Collections;
using SpectroWizard.dev;
using SpectroWizard.data;
using SpectroWizard.gui;
using SpectroWizard.data.lib;
using System.Media;

#region BUG List
/**
 * DONE: Выпавшие при стат обработке результатов промеры влияют на калибровочный график 
 */
#endregion
#region Changes log
/***************************************************************************************
 *   Change log:                                                                       *
 ***************************************************************************************
 * 3.0.3.75 -   Убран глюк приводящий с потере привязки
 * 3.0.3.74 -   Добавлен поиск аналитических линий 
 * 3.0.3.73 -   Изменино поведение программы в вычислении суммарного спектра при слишком 
 *            больших уровнях.
 * 3.0.3.72 -   Исправление ошибок связанных с добавлением новых прожигов в методику
 * 3.0.3.71 -   Подбор линий сравнения, стандартные образцы в неизвестных пробах
 * 3.0.3.70r -  Финальная версия в Луганске
 * 3.0.2.69 -   Добавлен механизм восстановления нуля и сброса приставки после измерения задания
 * 3.0.2.68 -   Финальная версия в Луганске
 * 3.0.2.67 -   Изменения в Луганске до привязки по атласу
 * 3.0.2.66 -   Устранены утечки памяти.
 * 3.0.2.65 -   Исправлены ошибки с большими фонтами и приглажено вывод калибр.чувств.
 * 3.0.2.62 -   Исправлена ошибка при измерении неизвестной пробы
 * 3.0.2.61 -   Отладка после Луганска
 * 3.0.2.60 -   Изменения в Луганске
 * 3.0.2.59 -   Изменения для Луганска
 * 3.0.2.58 -   Исправлена ошибка в подборе экспозиций и в работе с пустыми стандартами
 * 3.0.2.57 -   Большие изменения в структуре данных. Оптимизация по скорости.
 * 3.0.2.56 -   Запуск по вспышке
 * 3.0.2.55 -   Много мелких добавлений и исправлений
 * 3.0.2.54r -  После тестирования и отладки
 * 3.0.1.53 -   Убран эталонный спектр и добавлен профиль линии
 * 3.0.1.52 -   Убран спектр эталона длин волн. Переработан алгоритм вычисления.
 * 3.0.1.51 -   Добавлен специализированные диалог для выбора методики в измерениях
 * 3.0.1.50 -   Исправление ошибок в измерительном задании
 * 3.0.1.49 -   Добавлен полноценных Лог действий пользователя. Добавлено контекстное 
 *              меню
 * 3.0.1.48 -   Исправления имён в Bkp и при перезагрузки формул с большим к-м элем.
 * 3.0.1.47 -   Исправления и дополнения после тестирования в Комунаре
 * 3.0.1.46 -   Исправления и дополнения к первой версии. До полного тестирования.
 * 3.0.1.45r -  После тестирования.
 * 3.0.0.44 -   Добавлена автоматическая система резервного копирования данных.
 * 3.0.0.43 -   Добавлен учёт оценки прожига.
 * 3.0.0.42 -   Добавлена добавлены и улучшены алгоритмы проверки корректности спектров
 * 3.0.0.41 -   Добавлена ручная коррекция графиков
 * 3.0.0.40 -   Добавлена оценка прожига и проверка пустых мест
 * 3.0.0.39 -   Добавлен графический редактор экспозиций и отформатированы таблицы
 * 3.0.0.38 -   Добавлены предупреждения и проверка абсолютных путей
 * 3.0.0.37 -   Исправленные ошибки после превого предварительного тестирования
 * 3.0.0.36 -   Добавлено окно с внутренними результатами методик и заданий
 * 3.0.0.35 -   Добавлена печать результатов
 * 3.0.0.34 -   Создано измерительное задание
 * 3.0.0.33 -   Создана методика
 * 3.0.0.32 -   Проведена градуировка дисперсионного графика и привязка
 * 3.0.0.31 -   Установлен переключатель видов на поле просмотра спектров
 * 3.0.0.30 -   Снят привязочный спектр
 * 3.0.0.29 -   Проверена компенсация чувствительности
 * 3.0.0.28 -   Добавлено управление заливающим светом
 * 3.0.0.27 -   Исправление некоторых ошибок перед первым испытанием
 * 3.0.0.26 -   Добавлена база данных линий со встроенным компилятором
 * 3.0.0.25 -   Добавлено связь с приставками 
 * 3.0.0.24 -   Добавлено фоновое измерение нуля в методике 
 * 3.0.0.23 -   За дышала методика. Включая механизм коррекции дисперсии по эталонному 
 *              спектру
 * 3.0.0.22 -   Продолжение имплементации методики. Остановился на вычислении амплитуды
 *              линии
 * 3.0.0.21 -   Начало создания методики + Добавлены из предыдущей версии библиотека
 *              стандартрых образцов и список химических элементов
 * 3.0.0.20 -   Закончена калибровка оптической функции. Но не добавлена коррекция
 * 3.0.0.19 -   В механизм компенсации введена оценка лучшей аппаратной функции
 * 3.0.0.18 -   Начато введение компенсационных механизмов для оптики
 * 3.0.0.17 -   Добавлена проверка и компенсация сдвига
 ***************************************************************************************/
#endregion

namespace SpectroWizard
{
    class Common
    {
        public static void ClearMemory()
        {
            System.GC.Collect();
        }

        public static void Beep()
        {
            try
            {
                SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
                simpleSound.Play();
            }
            catch (Exception exx) { }

            try
            {
                SoundPlayer simpleSound = new SoundPlayer(@"completed.wav");//c:\Windows\Media\chimes.wav");
                simpleSound.Play();
            }
            catch (Exception ex)
            {
            }
        }

        #region Version....
        public static bool Debug
        {
            get
            {
                return VersionState == VersionStateTypes.Construct ||
                    Common.UserRole == Common.UserRoleTypes.Debuger;
            }
        }
        public static int TestedBuild = 3;
        public static int DevelopersBuild = 78;
        public static int TestBuild = 4364;
        public static string Version = "3.0."+TestedBuild+"."+DevelopersBuild+" ("+TestBuild+")";
        public enum VersionStateTypes
        {
            Construct,
            Testing,
            Release
        }
        public static VersionStateTypes VersionState = VersionStateTypes.Construct;
        //public static VersionStateTypes VersionState = VersionStateTypes.Testing;
        //public static VersionStateTypes VersionState = VersionStateTypes.Release;
        public static string VersionStateString
        {
            get
            {
                switch (VersionState)
                {
                    case VersionStateTypes.Construct: return "construct";
                    case VersionStateTypes.Release: return "final";
                    case VersionStateTypes.Testing: return "testing";
                    default: throw new Exception("Wrong type of the version state");
                }
            }
        }
        public static string Name = "Spectro Wizard";
        public static string Web = "www.spectrometer.com.ua";
        public static GeneratorControlForm GenForm = null;

        public static string ProgramFullInfo
        {
            get
            {
                return Name + " Ver:" + Version + "[" + VersionStateString + "] " + Web;
            }
        }
        #endregion

        #region Global names
        public const string LogCalcSectionName = "CalcLog";
        public const string LogCalcGraphSectionName = "CalcGraphLog";
        #endregion

        #region Font check
        public static string DefaultResultTableSpaces = "    ";
        public static Font[] DefaultResultTableFont;/* = {new Font(FontFamily.GenericSansSerif, Common.Env.DefaultFontSize,FontStyle.Underline),
                                  new Font(FontFamily.GenericSansSerif, Common.Env.DefaultFontSize,FontStyle.Bold),
                                 new Font(FontFamily.GenericSansSerif,9)};*/

        //public static int DefaultFntSize = 12;
        public static Font GetDefaultFont(Font orig)
        {
            return new Font(orig.FontFamily, Env.DefaultFontSize, orig.Style);
        }
        public static Font GetDefaultFont(Font orig,FontStyle fs)
        {
            return new Font(orig.FontFamily, Env.DefaultFontSize, fs);
        }
        public static void SetupFont(Form frm)
        {
            frm.Font = GetDefaultFont(frm.Font);
            if (frm.MainMenuStrip != null)
                frm.MainMenuStrip.Font = frm.Font;
            for (int i = 0; i < frm.Controls.Count; i++)
                CheckControlsMenu(frm.Controls[i], frm.Font);
        }

        public static void SetupFont(MenuStrip menu)
        {
            menu.Font = new Font(menu.Font.FontFamily, Env.DefaultFontSize, menu.Font.Style);;
        }

        static void CheckControlsMenu(Control frm, Font f)
        {
            for (int i = 0; i < frm.Controls.Count; i++)
            {
                if (frm.Controls[i] is MenuStrip)
                {
                    MenuStrip p = (MenuStrip)frm.Controls[i];
                    p.Font = f;
                }
                CheckControlsMenu(frm.Controls[i], f);
            }
        }
        #endregion

        #region Profiling
        static bool Profiling = true;
        static Hashtable ProfileTable = new Hashtable();
        public static void ProfileStart(string profile_name)
        {
            long now = DateTime.Now.Ticks;
            if (Profiling == false)
                return;
            ProfileTable.Add(profile_name, now);
        }

        public static void ProfileEnd(string profile_name)
        {
            long cur = DateTime.Now.Ticks;
            if (Profiling == false)
                return;
            if (ProfileTable.ContainsKey(profile_name) == false)
                Common.Log("Can't profile: '" + profile_name + "'");
            long tmp = (long)ProfileTable[profile_name];
            ProfileTable.Remove(profile_name);
            Common.Log("Profile: '" + profile_name + "' dlt_time = " + ((cur - tmp) / 10000.0) + "msk");
        }
        #endregion

        public static bool CancelFlag = false;
        public static bool ExtraSafe = true;

        public static bool PotiomkinRequest = true;
        public static bool IsPotiomkin{
            get
            {
                if (File.Exists("pd"))
                    return PotiomkinRequest;
                else
                    return false;
            }
        }

        public enum UserRoleTypes
        {
            Laborant,Metodist,Debuger,Sientist
        };
        public static UserRoleTypes UserRole = UserRoleTypes.Laborant;
        public static Dev Dev;
        public static Config Conf;
        public static DataBase Db;
        public static LineDb LDb;

        const string oldDbNameTunningFolder = "tunning";
        const string oldDbNameSienceSensFolder = "s_sens";
        const string oldDbNameMethodsFolder = "methods";
        const string oldDbNameMeasuringsFolder = "measurings";
        const string oldDbNameSystemFolder = "system";
        const string oldDbNameLinkingFolder = "linking";
        const string oldDbNameTestingFolder = "testing";
        const string oldDbNameStLibFolder = "st_lib";
        const string oldDbNameNulFolder = "nulls";
        static string[] oldDbStructure = 
            { 
                oldDbNameTunningFolder, 
                oldDbNameMethodsFolder, 
                oldDbNameMeasuringsFolder,
                oldDbNameSystemFolder,
                oldDbNameLinkingFolder,
                oldDbNameTestingFolder,
                oldDbNameStLibFolder,
                oldDbNameNulFolder,
                oldDbNameSienceSensFolder
            };

        public const string DbNameTunningFolder = "Тестовые измерения спектров";
        public const string DbNameSienceSensFolder = "Внутренние проверки чувствительности";
        public const string DbNameMethodsFolder = "Папка градуировок";
        public const string DbNameMeasuringsFolder = "Папка измерения неизвестных проб";
        public const string DbNameSystemFolder = "Системные настройки";
        public const string DbNameLinkingFolder = "Привязка";
        public const string DbNameTestingFolder = "Проверка системы регистрации";
        public const string DbNameStLibFolder = "Библиотека стандартных образцов";
        public const string DbNameNulFolder = "Калибровки фона";
        public const string DbNameProbSort = "Сортировка проб";
        public const string DbNameLyDb = "Характерные линии";
        static string[] DbStructure = 
            { 
                DbNameTunningFolder, 
                DbNameMethodsFolder, 
                DbNameMeasuringsFolder,
                DbNameSystemFolder,
                DbNameLinkingFolder,
                DbNameTestingFolder,
                DbNameStLibFolder,
                DbNameNulFolder,
                DbNameSienceSensFolder,
                DbNameProbSort,
                DbNameLyDb
            };

        public static void Reg(Control contr,string mls_const)
        {
            MLS.Reg(contr, mls_const);
            SpectroWizard.Log.Reg(mls_const,contr);
        }

        public const string DbObjectNamesLinkMatrixFile = "base_link_matrix";
        public const string DbObjectNamesOFuncFile = "ofk_base";
        public static void CreateDb()
        {
            Db = new DataBase(DbStructure,oldDbStructure);
        }

        public static SpAtlas SpectrAtlas;

        public static Env Env;
        public static string DBBaseStLib
        {
            get
            {
                return DataBase.BasePath + DbNameStLibFolder+"\\";
            }
        }
        
        public static string SpAtlasPath
        {
            get
            {
                return DataBase.BasePath;
            }
        }
        
        public static string DBBaseEnv
        {
            get
            {
                return DataBase.BasePath;
            }
        }
        
        public static string EnvPath
        {
            get
            {
                return DataBase.BasePath;
            }
        }

        public static GOSTDb GOSTDb,CUSTOMDb;
        public static LyDb LyData;
        public static bool IsRunning = false;
        public static bool IsClosing = false;

        public static string RecoveryDirName = "Recovery\\";
        public static void Start()
        {
            if (Directory.Exists(RecoveryDirName) == false)
                Directory.CreateDirectory(RecoveryDirName);
            else
                serv.DirectoryCopy(RecoveryDirName, ".", true);//*/

            SpectroWizard.Log.SetupPath("log.txt");
            Conf = new Config();
            CreateDb();
            try
            {
                SpectrAtlas = new SpAtlas();
                SpectrAtlas.Load();
            }
            catch
            {
            }
            try
            {
                Env = Env.Restore(EnvPath);
            }
            catch
            {
                Env = new Env();
            }
            try
            {
                LDb = new LineDb("lib\\data.bin");
            }
            catch
            {
                LDb = new LineDb();
            }
            try
            {
                LyData = new LyDb();
            }
            catch (Exception ex)
            {
                //Log(ex);
            }
            IsRunning = true;
            Dev = KnownDevList.GetDev(Conf.RegId, Conf.GenId,
                Conf.FillLId, Conf.GasId);
            Dev.Reg.InitNullCalibrator();
            DefaultResultTableFont = new Font[3];
            DefaultResultTableFont[0] = new Font(FontFamily.GenericSerif, Common.Env.DefaultFontSize,FontStyle.Underline);
            DefaultResultTableFont[1] = new Font(FontFamily.GenericSerif, Common.Env.DefaultFontSize, FontStyle.Bold);
            int fsize = Common.Env.DefaultFontSize;
            fsize = (int)(fsize * 0.8);
            if (fsize < 8)
                fsize = 8;
            DefaultResultTableFont[2] = new Font(FontFamily.GenericSerif, fsize);
            GOSTDb = new GOSTDb();
            GOSTDb.Init("lib\\gost\\");
            CUSTOMDb = new GOSTDb();
            CUSTOMDb.Init("lib\\custom\\");
        }

        public static Form GetTopForm()
        {
            return gui.MainForm.MForm;
        }

        public static void Stop()
        {
            IsRunning = false;
            if (Dev.Gen.IsConnected())
            {
                try { Dev.Gen.SetStatus(false); }
                catch { }
                try { Dev.Fill.SetFillLight(false); }
                catch { }
            }
            try
            {
                Env.Store(EnvPath);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            try
            {
                Common.MLS.Save();
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            try
            {
                Common.Log("Exit...");
                SpectroWizard.Log.Save();
            }
            catch
            {
            }
            if (LogCEx > 0 && File.Exists("no_log_bkp") == false)
                try
                {
                    if (Directory.Exists("log_bkp") == false)
                        Directory.CreateDirectory("log_bkp");

                    FileStream src_fs = new FileStream("log.txt", FileMode.Open, FileAccess.Read);
                    string name = "log_bkp\\log_" + DateTime.Now.ToString() + ".txt";
                    string fname = "";
                    for(int i = 0;i<name.Length;i++)
                        if(name[i] != ':')
                            fname += name[i];
                        else
                            fname += "_";
                    FileStream dst_fs = new FileStream(fname, FileMode.Create, FileAccess.Write);

                    byte[] src = new byte[src_fs.Length];
                    src_fs.Read(src, 0, src.Length);
                    src_fs.Close();

                    dst_fs.Write(src, 0, src.Length);
                    dst_fs.Flush();
                    dst_fs.Close();
                }
                catch
                {
                }
        }

        public static Font GraphLitleFont = new Font(FontFamily.GenericMonospace, 8);
        public static Font GraphNormalFont = new Font(FontFamily.GenericMonospace, 11);
        public static Font GraphBigFont = new Font(FontFamily.GenericMonospace, 30, FontStyle.Bold);

        static int LogCEx = 0;
        static int LogCMsg = 0;
        static int LogCWar = 0;
        static string LogLastText = "";
        static Color LogColor;
        static Font LogFont;
        static Font DefaultLogFont = new Font(FontFamily.GenericSansSerif, 9);
        static Font DefaultExceptionFont = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold | FontStyle.Underline);
        static Font DefaultWarFont = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Italic);
        public static FontServer FServ = new FontServer();
        public static void SetupLogInfo()
        {
            if (MainForm.MForm == null)
                return;
            //MainForm.MForm.StatusBarMainPanel.ForeColor = LogColor;
            //MainForm.MForm.StatusBarMainPanel.Font = LogFont;
            MainForm.MForm.StatusBarLog.Text = "M:" + LogCMsg + " E:" + LogCEx + " W:" + LogCWar;
            MainForm.MForm.SetupMsg(LogLastText, LogColor);//StatusBarMainPanel.Text = LogLastText;
        }

        public static void Log(Exception ex)
        {
            Log(ex, true);
        }

        public static void LogNoMsg(Exception ex)
        {
            Log(ex, false);
        }

        public static void LogTrace(string msg)
        {
            try
            {
                string tmsg = null;
                tmsg.Trim();
            }
            catch (Exception ex)
            {
                Log("StackTrace: '"+msg+"'"+serv.Endl+ex.StackTrace);
            }
        }

        public static void Log(Exception ex, bool show_ex)
        {
            LogCEx++;
            try
            {
                SpectroWizard.Log.Out(ex);
                SpectroWizard.Log.Save();
                LogLastText = "E:" + ex.Message;
                LogColor = Color.Red;
                LogFont = DefaultExceptionFont;
                SetupLogInfo();
                if(show_ex == true)
                    util.LogException.Show(ex);
            }
            catch
            {
            }
        }

        public static void UserLog(Control contr,string str)
        {
            try
            {
                Control c = contr;
                while (c != null)
                {
                    if (c.Visible == false)
                        return;
                    c = c.Parent;
                }
                LogCMsg++;
                if (contr.Text != null)
                    SpectroWizard.Log.OutUf(">>>> " + contr.Name + "'" + contr.Text + "'" + "[" + str + "]");
                else
                {
                    if(contr.Tag != null)
                        SpectroWizard.Log.OutUf(">>>> " + contr.Name + "'" + contr.Text + "'" + " [" + str + "]");
                    else
                        SpectroWizard.Log.OutUf(">>>> " + contr.Name + " [" + str + "]");
                }
                LogLastText = "";
                //LogColor = SystemColors.ControlText;
                LogFont = DefaultLogFont;
                SetupLogInfo();
            }
            catch
            {
            }
        }

        public static void UserLog(string str)
        {
            try
            {
                LogCMsg++;
                SpectroWizard.Log.OutUf(">>>> " + str);
                LogLastText = "";
                LogFont = DefaultLogFont;
                SetupLogInfo();
            }
            catch
            {
            }
        }

        public static void Log(string str)
        {
            LogCMsg++;
            try
            {
                SpectroWizard.Log.Out(str);
                LogLastText = str;
                LogColor = SystemColors.ControlText;
                LogFont = DefaultLogFont;
                SetupLogInfo();
            }
            catch
            {
            }
        }

        public static void LogWar(string str)
        {
            LogCWar++;
            try
            {
                SpectroWizard.Log.Out(str);
                LogLastText = "W:" + str;
                LogColor = Color.Blue;
                LogFont = DefaultWarFont;
                SetupLogInfo();
            }
            catch
            {
            }
        }

        static public Mls MLS;

        static public Cursor GetCursor(string name)
        {
            Cursor c = new Cursor("images\\cur\\" + name);
            return c;
        }
    }

    public class Config
    {
        public uint RegId = 0, 
            GenId = 0,
            FillLId = 0,
            GasId = 0;
        public byte IP1 = 192, IP2 = 168, IP3 = 143, IP4 = 1;
        public ushort Port = 14377;
        public string DbPath = "Данные\\";
        public float MinTick = 0.04F;
        public bool UseGoodNul = true;
        public byte Divider = 2;
        public byte Divider2
        {
            get
            {
                switch (Divider)
                {
                    case 1: return 0;
                    case 2: return 1;
                    case 4: return 2;
                    case 8: return 3;
                    case 16: return 4;
                    case 32: return 5;
                    default: throw new Exception("Wrorng divider value: "+Divider);
                }
            }
        }
        public float MaxLevel = 18000;
        public string BkpPath = "";
        public bool BkpWeek = false;
        public bool BkpYear = false;
        public DateTime BkpWeekDate = new DateTime(1,1,1);
        public DateTime BkpYearDate = new DateTime(1, 1, 1);
        public int LastStartedBuild = 0;
        public bool UseStatisic = false;
        public bool UseOptickK = false;
        public bool UseLineAmpl = false;
        public int BlakPixelStart = 0;
        public int BlakPixelEnd = 0;
        public bool BlankSub = false;
        public int NulHistory = 1200;
        public int ValidSensorFrom = 1;
        public int ValidSensorTo = 32;
        public string PriborName = "None";
        public float DefaultExposition = 0.5F;
        public int ValidSensorDiff = 10;
        public int ValidFillLightAdd = 5;
        public int ValidMemoryUsage = 800;
        public int MultFactor = 2;
        public string USBOrientation = "1 -2 3 -4 5 -6 7 -8 9 -10 11 -12 13 -14 15";
        public int MeasuringTimeOut = 0;
        public int[] USBOrientationVals
        {
            get
            {
                String[] vals = USBOrientation.Split(' ');
                int[] ret = new int[vals.Length];
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = int.Parse(vals[i]);
                return ret;
            }
        }

        public Config()
        {
            if (File.Exists("config.bin"))
            {
                FileStream fs = new FileStream("config.bin", FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                string type = br.ReadString();
                if (type.Equals("binconf") == false)
                {
                    fs.Close();
                    throw new Exception(Common.MLS.Get("Errors", "Wrong config file. Remove it."));
                }
                RegId = br.ReadUInt32();
                GenId = br.ReadUInt32();
                IP1 = br.ReadByte(); IP2 = br.ReadByte(); IP3 = br.ReadByte(); IP4 = br.ReadByte();
                Port = br.ReadUInt16();
                DbPath = br.ReadString();
                MinTick = br.ReadSingle();
                UseGoodNul = br.ReadBoolean();
                try
                {
                    Divider = br.ReadByte();
                    MaxLevel = br.ReadSingle();
                    FillLId = br.ReadUInt32();
                    GenId = br.ReadUInt32();
                    GasId = br.ReadUInt32();

                    BkpPath = br.ReadString();
                    BkpWeek = br.ReadBoolean();
                    BkpYear = br.ReadBoolean();
                    BkpWeekDate = new DateTime(br.ReadInt64());
                    BkpYearDate = new DateTime(br.ReadInt64());
                    LastStartedBuild = br.ReadInt32();
                    UseStatisic = br.ReadBoolean();
                    UseOptickK = br.ReadBoolean();
                    BlakPixelStart = br.ReadInt32();
                    BlakPixelEnd = br.ReadInt32();
                    BlankSub = br.ReadBoolean();
                    UseLineAmpl = br.ReadBoolean();
                    NulHistory = br.ReadInt32();
                    ValidSensorFrom = br.ReadInt32();
                    ValidSensorTo = br.ReadInt32();
                    PriborName = br.ReadString();
                    DefaultExposition = br.ReadSingle();
                    ValidSensorDiff = br.ReadInt32();
                    if (ValidSensorDiff < 2 || ValidSensorDiff > 25) ValidSensorDiff = 10;
                    ValidFillLightAdd = br.ReadInt32();

                    ValidMemoryUsage = br.ReadInt32();
                    MultFactor = br.ReadInt32();
                    USBOrientation = br.ReadString();
                    MeasuringTimeOut = br.ReadInt32();
                }
                catch(Exception ex)
                {
                    Common.LogNoMsg(ex);
                }
                fs.Close();
            }
            else
            {
                LastStartedBuild = Common.DevelopersBuild;
            }
        }

        public void Save()
        {
            FileStream fs = new FileStream("config.bin", FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write("binconf");
            bw.Write(RegId);
            bw.Write(GenId);
            bw.Write(IP1); bw.Write(IP2); bw.Write(IP3); bw.Write(IP4);
            bw.Write(Port);
            bw.Write(DbPath);
            //DataBase.BasePath = DbPath;
            bw.Write(MinTick);
            bw.Write(UseGoodNul);
            bw.Write(Divider);
            bw.Write(MaxLevel);
            bw.Write(FillLId);
            bw.Write(GenId);
            bw.Write(GasId);
            bw.Write(BkpPath);
            bw.Write(BkpWeek);
            bw.Write(BkpYear);
            bw.Write(BkpWeekDate.Ticks);
            bw.Write(BkpYearDate.Ticks);
            LastStartedBuild = Common.DevelopersBuild;
            bw.Write(LastStartedBuild);
            bw.Write(UseStatisic);
            bw.Write(UseOptickK);
            bw.Write(BlakPixelStart);
            bw.Write(BlakPixelEnd);
            bw.Write(BlankSub);
            bw.Write(UseLineAmpl);
            bw.Write(NulHistory);
            bw.Write(ValidSensorFrom);
            bw.Write(ValidSensorTo);
            bw.Write(PriborName);
            bw.Write(DefaultExposition);
            bw.Write(ValidSensorDiff);
            bw.Write(ValidFillLightAdd);
            bw.Write(ValidMemoryUsage);
            bw.Write(MultFactor);
            bw.Write(USBOrientation);
            bw.Write(MeasuringTimeOut);

            bw.Flush();
            bw.Close();

            Common.CreateDb();
            //Common.Db = new DataBase();
        }
    }
}
