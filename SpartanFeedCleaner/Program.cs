using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpartanFeedCleaner
{
    class Program
    {
        static void Main(string[] args)
        {

            if (File.Exists("C:\\BRdata\\XML\\IMPORT.ISI"))
            {
                if (new FileInfo("C:\\BRdata\\XML\\IMPORT.ISI").Length == 0)
                {
                    File.Delete("C:\\BRdata\\XML\\IMPORT.ISI");
                }
                else
                {
                    ISI("IMPORT.ISI");
                }
            }

            // if (args.Length == 0) ISI("IMPORT.ISI");


            if (!File.Exists("C:\\BRdata\\XML\\IMPORT.ISI"))
            {
                return;
            }
        }

        private static string CatDept(string Category)
        {

            //string result1 = "0001";
            Int32 Category1;
            Category1 = Convert.ToInt32(Category);
            if (Category1 < 10) { return System.Configuration.ConfigurationManager.AppSettings.Get("Bakery"); }
            else if (Category1 >= 10 && Category1 < 176) { return System.Configuration.ConfigurationManager.AppSettings.Get("Grocery"); }
            else if (Category1 >= 176 && Category1 < 250) { return System.Configuration.ConfigurationManager.AppSettings.Get("Taxable Grocery"); }
            else if (Category1 >= 250 && Category1 < 300) { return System.Configuration.ConfigurationManager.AppSettings.Get("Frozen Foods"); }
            else if (Category1 >= 300 && Category1 < 400) { return System.Configuration.ConfigurationManager.AppSettings.Get("Dairy"); }
            else if (Category1 >= 400 && Category1 < 450) { return System.Configuration.ConfigurationManager.AppSettings.Get("Meat"); }
            else if (Category1 >= 450 && Category1 < 460) { return System.Configuration.ConfigurationManager.AppSettings.Get("Seafood"); }
            else if (Category1 >= 460 && Category1 < 470) { return System.Configuration.ConfigurationManager.AppSettings.Get("Produce"); }
            else if (Category1 >= 470 && Category1 < 476) { return System.Configuration.ConfigurationManager.AppSettings.Get("Floral"); }
            else if (Category1 >= 476 && Category1 < 490) { return System.Configuration.ConfigurationManager.AppSettings.Get("Reserved for future"); }
            else if (Category1 >= 490 && Category1 < 500) { return System.Configuration.ConfigurationManager.AppSettings.Get("Garden Center"); }
            else if (Category1 >= 500 && Category1 < 540) { return System.Configuration.ConfigurationManager.AppSettings.Get("Beer"); }
            else if (Category1 >= 540 && Category1 < 580) { return System.Configuration.ConfigurationManager.AppSettings.Get("Wine"); }
            else if (Category1 >= 580 && Category1 < 600) { return System.Configuration.ConfigurationManager.AppSettings.Get("Liquor"); }
            else if (Category1 >= 600 && Category1 < 650) { return System.Configuration.ConfigurationManager.AppSettings.Get("HBC"); }
            else if (Category1 >= 650 && Category1 < 700) { return System.Configuration.ConfigurationManager.AppSettings.Get("GM Hard"); }
            else if (Category1 >= 700 && Category1 < 850) { return System.Configuration.ConfigurationManager.AppSettings.Get("GM Soft"); }
            else if (Category1 >= 850 && Category1 < 950) { return System.Configuration.ConfigurationManager.AppSettings.Get("GM Soft"); }
            else if (Category1 >= 950 && Category1 < 976) { return System.Configuration.ConfigurationManager.AppSettings.Get("Supplies"); }
            else if (Category1 > 976) { return System.Configuration.ConfigurationManager.AppSettings.Get("Misc"); }
            else { return "0010"; }
            //return "0001";
        }

        private static string DiscCode(string DiscReason)
        {
            if (DiscReason.ToUpper().Contains("DUE TO SLOW")) { return "1"; }
            else if (DiscReason.ToUpper().Contains("CATEGORY ANALYSIS")) { return "2"; }
            else if (DiscReason.ToUpper().Contains("CAO")) { return "3"; }
            else if (DiscReason.ToUpper().Contains("MANUFACTURER")) { return "4"; }
            else if (DiscReason.ToUpper().Contains("PROGRAM ELIMINATED")) { return "5"; }
            else if (DiscReason.ToUpper().Contains("PACK/SIZE")) { return "6"; }
            else if (DiscReason.ToUpper().Contains("DUE TO SLOW")) { return "1"; }
            else { return "1"; }
        }


        //static void ISI(string filename, string dept1 = " 0", string dept2 = " 0", string dept3 = " 0")
        private static void ISI(string filename)
        {
            //stream reader here
            using (FileStream fs = new FileStream(Path.Combine("C:\\BRdata\\XML", filename), FileMode.Open, FileAccess.Read, FileShare.None))
            using (StreamReader sr = new StreamReader(fs))

            //stream writer(s) here
            using (FileStream fo = new FileStream(Path.Combine("C:\\Brdata\\XML", "Import_Revised.ISI"), FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter so = new StreamWriter(fo))

            using (FileStream fpo = new FileStream(Path.Combine("C:\\Brdata\\XML\\Feed", "Fprice.dat"), FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter sfpo = new StreamWriter(fpo))

            using (FileStream apo = new FileStream(Path.Combine("C:\\Brdata\\XML\\Feed", "ItemChanges.dat"), FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter sapo = new StreamWriter(apo))

            using (FileStream cpo = new FileStream(Path.Combine("C:\\Brdata\\XML\\Feed", "Categories.dat"), FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter scpo = new StreamWriter(cpo))

            using (FileStream copo = new FileStream(Path.Combine("C:\\Brdata\\XML\\Feed", "CostChanges.dat"), FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter scopo = new StreamWriter(copo))

            using (FileStream dpo = new FileStream(Path.Combine("C:\\Brdata\\XML\\Feed", "Discontinued.dat"), FileMode.Append, FileAccess.Write, FileShare.None))
            using (StreamWriter sdpo = new StreamWriter(dpo))

            {
                sr.ReadLine();
                bool infprice = false;
                bool inaddrplitem = false;
                bool inaddrplcatcls = false;
                bool inaddrplfcost = false;
                bool indisco = false;

                //variables from app.config
                string dept1 = System.Configuration.ConfigurationManager.AppSettings.Get("dept1");
                string dept2 = System.Configuration.ConfigurationManager.AppSettings.Get("dept2");
                string dept3 = System.Configuration.ConfigurationManager.AppSettings.Get("dept3");
                string dept4 = System.Configuration.ConfigurationManager.AppSettings.Get("dept4");
                string dept5 = System.Configuration.ConfigurationManager.AppSettings.Get("dept5");
                string dept6 = System.Configuration.ConfigurationManager.AppSettings.Get("dept6");

                string idept1 = System.Configuration.ConfigurationManager.AppSettings.Get("idept1");
                string idept2 = System.Configuration.ConfigurationManager.AppSettings.Get("idept2");
                string idept3 = System.Configuration.ConfigurationManager.AppSettings.Get("idept3");
                string idept4 = System.Configuration.ConfigurationManager.AppSettings.Get("idept4");
                string idept5 = System.Configuration.ConfigurationManager.AppSettings.Get("idept5");
                string idept6 = System.Configuration.ConfigurationManager.AppSettings.Get("idept6");

                string SKUPad = System.Configuration.ConfigurationManager.AppSettings.Get("SKUPad");
                string DiscoLimit = System.Configuration.ConfigurationManager.AppSettings.Get("DiscoLimit");

                while (true)
                {
                    string spartan_input = sr.ReadLine();
                    if (spartan_input == null) break;

                    try
                    {
                        string new_line = spartan_input;

                        if (new_line.ToUpper().Contains("*HEADER"))
                        {
                            infprice = false;
                            inaddrplitem = false;
                            inaddrplcatcls = false;
                            inaddrplfcost = false;
                            indisco = false;
                            so.WriteLine($"{new_line}");
                            //CatDept("0001");
                            //return;
                        }

                        else if (new_line.ToUpper().Contains("CATCLS"))
                        {
                            inaddrplcatcls = true;
                            scpo.WriteLine($"{new_line}");
                        }

                        else if (inaddrplcatcls)
                        {
                            try
                            {
                                //do nothing because skipping selected departments
                                string categoryprefix = new_line.Substring(1, 4);
                                string categorysuffix = new_line.Substring(7, 4);
                                string categorycode = "0" + categoryprefix.Trim() + "0" + categorysuffix.Trim();
                                string categoryname = (new_line.Substring(24, 30).Trim().Replace(" ", ""));
                                scpo.WriteLine($"{categorycode}" + "     " + categoryname);
                            }

                            catch
                            {
                                scpo.WriteLine($"{new_line}");
                            }

                        }

                        else if (new_line.ToUpper().Contains("PRODNT"))
                        {
                            indisco = true;
                            // sdpo.WriteLine(new_line);
                        }

                        else if (indisco)
                        {
                            try
                            {
                                DateTime DiscoDate = DateTime.Parse(new_line.Substring(30, 10));
                                DateTime DiscoToday = DateTime.Now;
                                double DiscoDayCount = (DiscoDate - DiscoToday).TotalDays;
                                DiscoDayCount = Math.Abs(DiscoDayCount);
                                //sdpo.WriteLine(DiscoToday);
                                //sdpo.WriteLine(DiscoDayCount);

                                if (DiscoDayCount > Int32.Parse(DiscoLimit))
                                {
                                    //do nothing you idiots
                                }

                                else
                                {
                                    string reason = DiscCode(new_line.Substring(41, 30));
                                    sdpo.WriteLine($"{new_line}Y~{reason}".Replace("  ", ""));
                                }
                            }
                            catch
                            {
                                //do nothing
                            }

                            // }



                        }



                        else if (new_line.ToUpper().Contains("FPRICE"))
                        {
                            infprice = true;
                            sfpo.WriteLine("*HEADER");
                            sfpo.WriteLine($"{new_line}");
                            //so.WriteLine("Fprice = true ");
                        }
                        else if (infprice)
                        {
                            try
                            {
                                string DateRange = System.Configuration.ConfigurationManager.AppSettings.Get("DateRange");
                                int DateRange2 = Int32.Parse(DateRange);

                                DateTime todaydate = DateTime.Now;
                                string fpricedate = new_line.Substring(34, 10);
                                DateTime fpricedate_new = DateTime.Parse(fpricedate);
                                double daycount = (todaydate - fpricedate_new).TotalDays;
                                daycount = Math.Abs(daycount);

                                if (new_line.Substring(62, 2) == dept1 | new_line.Substring(62, 2) == dept2 | new_line.Substring(62, 2) == dept3 |
                                    new_line.Substring(62, 2) == dept4 | new_line.Substring(62, 2) == dept5 | new_line.Substring(62, 2) == dept6 | daycount > DateRange2)
                                {
                                    //do nothing because skipping selected departments
                                }
                                else
                                {

                                    DateTime today = Convert.ToDateTime(new_line.Substring(34, 10));
                                    // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
                                    int daysUntilEffective = (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("EffectiveDate")) - (int)today.DayOfWeek + 7) % 7;
                                    DateTime nexteffective = today.AddDays(daysUntilEffective);
                                    string new_effective_date = nexteffective.ToString("MM/dd/yyyy");

                                    sfpo.WriteLine($"{new_line.Substring(0, 34)}{new_effective_date}~{new_line.Substring(45, 20)}");

                                }
                            }

                            catch
                            {
                                //added this here

                                try
                                {
                                    if (new_line.ToUpper().Contains("*HEADER"))
                                    {
                                        infprice = false;
                                        break;
                                    }
                                }
                                catch
                                {
                                    sfpo.WriteLine("error reading data");
                                }

                                //sfpo.WriteLine($"{new_line.Substring(0, 100)}xx{new_line.Substring(36, 44)}xx{new_line.Substring(45, 65)}");
                                sfpo.WriteLine($"{new_line}");
                                //sfpo.WriteLine($"{new_line.Substring(0, 35)}xx{new_line.Substring(36, 44)}xx{new_line.Substring(45, 65)}");
                                //so.WriteLine("error");
                            }

                        }

                        else if (new_line.ToUpper().Contains("ADDRPLITEM"))
                        {
                            string AddRPLMode = System.Configuration.ConfigurationManager.AppSettings.Get("ADDRPLITEM_MODE");
                            if (AddRPLMode == "True")
                            {
                                inaddrplitem = true;
                                //so.WriteLine("mode changed to addrplitem");
                                sapo.WriteLine("*HEADER");
                                sapo.WriteLine($"{new_line}");
                            }
                            else
                            {
                                //do nothing
                                so.WriteLine($"{new_line}");
                            }

                        }

                        else if (inaddrplitem)
                        {
                            try
                            {
                                //string skip_departments = System.Configuration.ConfigurationManager.AppSettings.Get("ADDRPL_SKIP_DEPARTMENTS");
                                //if (skip_departments == "True")
                                {
                                    if (new_line.Substring(31, 2) == idept1 | new_line.Substring(31, 2) == idept2 | new_line.Substring(31, 2) == idept3 |
                                    new_line.Substring(31, 2) == idept4 | new_line.Substring(31, 2) == idept5 | new_line.Substring(31, 2) == idept6)
                                    {
                                        //do nothing
                                        //so.WriteLine("doing nothing loop");
                                        //return;
                                    }
                                    else
                                    {
                                        //so.WriteLine(new_line.Substring(31, 2));
                                        // string AddDepartment = System.Configuration.ConfigurationManager.AppSettings.Get("AddDepartment");
                                        if (System.Configuration.ConfigurationManager.AppSettings.Get("AddDepartment") == "True")
                                        {
                                            string Category = new_line.Substring(123, 4).Trim();
                                            string Dept = CatDept(Category);
                                            string e_line = $"{new_line} {Dept}~Y~".Replace("  ", "").Replace("~ ", "~").Replace(" ~", "~").Replace(SKUPad, "");
                                            //string f_line = e_line.Replace("  ", "").Replace("~ ", "~").Replace(" ~", "~").Replace("00000000", "");
                                            //f_line = f_line.Replace("00000000", "");
                                            sapo.WriteLine(e_line);

                                        }
                                        else
                                            sapo.WriteLine(new_line);


                                        //debugging call outs
                                        //so.WriteLine("Category String " + Category);
                                        //so.WriteLine("Department#  " + Dept);
                                    }


                                }
                                //else
                                //    sapo.WriteLine(new_line);
                            }

                            catch
                            {
                                try
                                {
                                    if (new_line.ToUpper().Contains("HEADER*"))
                                    {
                                        infprice = false;
                                        inaddrplitem = false;
                                        break;
                                    }
                                }
                                catch
                                {
                                    so.WriteLine("error reading data");
                                }
                                so.WriteLine($"{new_line}");
                                //so.WriteLine("error");
                            }

                        }

                        else if (new_line.ToUpper().Contains("ADDRPLFCOST"))
                        {
                            inaddrplfcost = true;
                            scopo.WriteLine($"{new_line}");
                        }

                        else if (inaddrplfcost)
                        {
                            try
                            {
                                //scopo.WriteLine($"{new_line.Substring(22,2)}");   Used this to debug that column 22 was too far in reading department/supplier record
                                if (new_line.Substring(21, 2) == dept1 | new_line.Substring(21, 2) == dept2 | new_line.Substring(21, 2) == dept3 |
                                    new_line.Substring(21, 2) == dept4 | new_line.Substring(21, 2) == dept5 | new_line.Substring(21, 2) == dept6)
                                {
                                    //do nothing
                                    //so.WriteLine("doing nothing loop");
                                    //return;
                                }
                                else
                                {
                                    //so.WriteLine(new_line.Substring(31, 2));
                                    string RPLFCOST_MODE = System.Configuration.ConfigurationManager.AppSettings.Get("RPLFCOST_MODE");
                                    if (RPLFCOST_MODE == "True")
                                    {
                                        //string Category = new_line.Substring(123, 4).Trim();
                                        // string Dept = CatDept(Category);
                                        // string e_line = ($"{new_line}" + "  " + $"{Dept}" + "~ ");
                                        string f_line = new_line.Replace("  ", "").Replace("~ ", "~").Replace(" ~", "~");
                                        f_line = f_line.Replace("00000000", "");
                                        scopo.WriteLine($"{ f_line}");

                                    }
                                    else
                                        scopo.WriteLine($"{new_line}");


                                    //debugging call outs
                                    //so.WriteLine("Category String " + Category);
                                    //so.WriteLine("Department#  " + Dept);
                                }

                            }

                            catch
                            {
                                try
                                {
                                    if (new_line.ToUpper().Contains("HEADER*"))
                                    {
                                        infprice = false;
                                        break;
                                    }
                                }
                                catch
                                {
                                    so.WriteLine("error reading data");
                                }
                                so.WriteLine($"{new_line}");
                                //so.WriteLine("error");
                            }
                        }


                        else so.WriteLine($"{new_line}");


                        // so.WriteLine($"{new_line}");
                    }
                    catch
                    {

                        break;
                        //do nothing if error
                    }


                    // temp.ToString();

                    //so.WriteLine(upc);

                }
                sfpo.Dispose();
                sapo.Dispose();
                scopo.Dispose();
                scpo.Dispose();
                fs.Dispose();
                fo.Dispose();
                apo.Dispose();
                fpo.Dispose();

                //Clean up duplicate lines if necessary
                //string[] lines = File.ReadAllLines("C:\\BRdata\\XML\\Feed\\Fprice.dat");
                //File.WriteAllLines("C:\\BRdata\\XML\\Feed\\Fprice2.dat", lines.Distinct().ToArray());

                //if (File.Exists("C:\\BRdata\\XML\\IMPORT.ISI"))
                //{
                //    File.Delete("C:\\BRdata\\XML\\IMPORT.ISI");
                //}
            }

        }
    }


}

