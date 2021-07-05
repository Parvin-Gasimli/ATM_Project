using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_project
{
    class Program
    {
        static void Main(string[] args)
        { }
            abstract class Bank // Stub Bank  
        {
            protected string acc_no;
            protected string acc_ty;
            protected double balance;
            public static double WITHDRAW_LIMIT = 1000;


            public Bank(string acc_no, string acc_type, double balance)
            {
                this.acc_no = acc_no;
                this.acc_ty = acc_type;
                this.balance = balance;
            }
        }


        class usrDetails : Bank
        {
            private string name;
            private int TOTAL_TRANX;
            private int pin;
            private int _trail;
            private int _atm_no;
            public DateTime? last_access;

            // pass parameter to base consturctor  
            public usrDetails(int atm_no, string acc_no, string acc_type, string name, double balance, int pin)
              : base(acc_no, acc_type, balance)
            {
                this.atm_no = atm_no;
                this.name = name;
                this.pin = pin;
                last_access = null;
            }

            public string[] Trx_date = new string[5];
            public double[] Trx_amt = new double[5];
            public string[] Trx_mode = new string[5];
            public int Trx_count = 0;

            public string[] Trx_mini_date = new string[5];
            public double[] Trx_mini_amt = new double[5];
            public string[] Trx_mini_mode = new string[5];
            public int Trx_mini_count = 0;



            public string usrName
            {
                get { return name; }
                set { name = value; }
            }

            public string acc_type
            {
                get { return acc_ty; }
                set { acc_ty = value; }
            }

            public string usrAcc_no
            {
                get { return acc_no; }
                set { acc_no = value; }
            }

            public int usrMaxTrail
            {
                get { return _trail; }
                set { _trail = value; }
            }

            public int usrPin
            {
                get { return pin; }
                set { pin = value; }
            }

            public int atm_no
            {
                get { return _atm_no; }
                set { _atm_no = value; }
            }

            public int Tot_Tranx
            {
                get { return TOTAL_TRANX; }
                set { TOTAL_TRANX = value; }
            }

            public double bal
            {
                get { return balance; }
                set { balance = value; }
            }

        }



        class ATM
        {
            static public DateTime dt = DateTime.Now;
            public usrDetails[] usr = new usrDetails[10];

            static string ATM_ID = "SBPX080283";
            static string ATM_LOC = "SILLICON VALLEY";

            public ATM()
            {
                // user details stub  
                usr[0] = new usrDetails(1234, "SBPxxx2093", "Savings", "Tuhin Bagh", 40000, 1122);
                usr[1] = new usrDetails(1111, "ICICxx8690", "Current", "Basu Kammar", 15000, 1111);

                usr[2] = new usrDetails(2222, "HDFCxx8690", "Savings", "Sheldon Cooper", 45000, 2222);
                usr[3] = new usrDetails(3333, "ICICxx8690", "Current", "Fred Johson", 15000, 3333);

                usr[4] = new usrDetails(4444, "HDFCxx8690", "Savings", "Ted Mosbi", 50000, 4444);
                usr[5] = new usrDetails(5555, "ICICxx8690", "Current", "Fred Johson", 65000, 5555);

                usr[6] = new usrDetails(6666, "HDFCxx8690", "Savings", "Marshall Eriksen", 85000, 6666);
                usr[7] = new usrDetails(7777, "ICICxx8690", "Current", "Robin White", 95000, 7777);

                usr[8] = new usrDetails(8888, "HDFCxx8690", "Savings", "Penny Smith", 75000, 8888);
                usr[9] = new usrDetails(9999, "ICICxx8690", "Current", "Barney Stinson", 10000, 9999);

            }

            //ATM Specific UI  
            public static void UI_atm_prompt(String msg)
            {
                //Start.xxxx is nothing but accessing the static methods and properties of Start class!  
                Start.main_header();
                Console.WriteLine(Start.st + msg + "\n\n");
                Start.footer();
                Console.Write(Start.st + "Press <any> key to return:");
                Console.ReadKey();

            }

            public static void UI_loading()
            {
                Start.main_header();
                Console.WriteLine("\n\n\t\t  Transaction is being processed!\n\n");
                Console.Write("\t\t\t ");

                for (int x = 0; x < 20; x++)
                {
                    Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252); // for extended ascii  
                    System.Threading.Thread.Sleep(50);
                    Console.Write("{0}", (char)178);
                }


            }

            public void deposit(int atm_numb)
            {

                int i = 0;
                bool check = false;
                for (int j = 0; j < usr.Length; j++)
                {
                    if (usr[j].atm_no == atm_numb) // valid acc_no  
                    {
                        check = true;
                        i = j;
                    }
                }

            CLEAR:
                if (check)
                {
                    if (usr[i].last_access != null) // Halt user for a day from using ATM after he comepletes 10 transactions  
                    {
                        DateTime cur_time = DateTime.Now;
                        TimeSpan duration = DateTime.Parse(cur_time.ToString()) - (DateTime.Parse(usr[i].last_access.ToString()));
                        int day = (int)Math.Round(duration.TotalDays);

                        if (day == 0) // if same day  
                        {
                            Start.UI_error("Transaction limit\n\t\t\t exceeded for today!");
                            return;
                        }
                        else
                        {
                            usr[i].last_access = null;
                            goto CLEAR;
                        }

                    }

                    Start.main_header();
                    Console.WriteLine("\t\t\t  Account Type: " + usr[i].acc_type + " \n\n");
                    Console.Write("\t\t Insert the Amout: ");
                    try
                    {
                        double temp = double.Parse(Console.ReadLine());

                        if (temp >= 100)
                        {
                            if (temp <= Bank.WITHDRAW_LIMIT)
                            {
                                usr[i].bal += temp;
                                usr[i].Tot_Tranx++;

                                DateTime d = DateTime.Now;
                                record_trx(i, d.ToString("d"), temp, "Deposit"); // record trasaction*/  

                                UI_loading();

                                if (usr[i].Tot_Tranx == 10)
                                {
                                    usr[i].last_access = DateTime.Now; // last transaction time   
                                }
                            }
                            else
                            {
                                Start.UI_error("Funds can't exceed Rs.1000\n\t\t\t in single transaction!");
                                return;
                            }
                        }
                        else
                        {
                            Start.UI_error("Invalid Fund!");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        Start.UI_error("Only numbrers are allowed!");
                        return;
                    }
                }
            }

            public void withdraw(int atm_numb)
            {
                int i = 0;
                bool check = false;
                for (int j = 0; j < usr.Length; j++)
                {
                    if (usr[j].atm_no == atm_numb) // valid acc_no  
                    {
                        check = true;
                        i = j;
                    }
                }

                if (check)
                {

                CLEAR:
                    if (usr[i].last_access != null) // Halt user for a day from using ATM after he comepletes 10 transactions  
                    {
                        DateTime cur_time = DateTime.Now;
                        TimeSpan duration = DateTime.Parse(cur_time.ToString()) - (DateTime.Parse(usr[i].last_access.ToString()));
                        int day = (int)Math.Round(duration.TotalDays);

                        if (day == 0) // if same day  
                        {
                            Start.UI_error("Transaction limit\n\t\t\t exceeded for today!");
                            return;
                        }
                        else
                        {
                            usr[i].last_access = null;
                            goto CLEAR;
                        }

                    }

                    if (usr[i].bal <= 100)
                    {
                        Start.UI_error("Insuffcient Fund!");
                    }
                    else
                    {
                        Start.main_header();
                        Console.WriteLine("\t\t\t  Account Type: " + usr[i].acc_type + " \n\n");
                        Console.Write("\t\t Enter the Amout: ");
                        try
                        {
                            double temp = double.Parse(Console.ReadLine());
                            if (temp >= 100)
                            {
                                if (temp <= Bank.WITHDRAW_LIMIT)
                                {
                                    usr[i].bal -= temp;
                                    usr[i].Tot_Tranx++;

                                    DateTime d = DateTime.Now;
                                    record_trx(i, d.ToString("d"), temp, "Withdraw"); // record trasaction*/  

                                    UI_loading();

                                    if (usr[i].Tot_Tranx == 10)
                                    {
                                        usr[i].last_access = DateTime.Now; // last transaction time   
                                    }
                                }
                                else
                                {
                                    Start.UI_error("Funds can't exceed Rs.1000\n\t\t\t in single transaction!");
                                    return;
                                }
                            }
                            else
                            {
                                Start.UI_error("Invalid Fund!");
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            Start.UI_error("Only numbrers are allowed!");
                            return;
                        }

                    }
                }

            }

            public void balance(int atm_numb)
            {
                int i = 0;
                bool check = false;
                for (int j = 0; j < usr.Length; j++)
                {
                    if (usr[j].atm_no == atm_numb) // valid acc_no  
                    {
                        check = true;
                        i = j;
                    }
                }

                if (check)
                {
                    UI_atm_prompt(Start.xst + "User: " + usr[i].usrName + ".\n\n\t\t\tAccout Type: " + usr[i].acc_type + "\n"
                            + Start.mt + "Balace: Rs." + usr[i].bal);
                }
            }

            public void tnfr_fund(int atm_numb)
            {
                int i = 0;
                bool check = false;
                for (int j = 0; j < usr.Length; j++)
                {
                    if (usr[j].atm_no == atm_numb) // valid acc_no  
                    {
                        check = true;
                        i = j;
                    }
                }

                if (check)
                {
                    int pae = 0;
                    bool flag = false;
                    Start.main_header();
                    Console.WriteLine("\t\t\t  User: " + usr[i].usrName + " \n\n");
                    Console.Write("\t\t Enter payee CARD no: ");
                    try
                    {
                        int payee_atm_no = int.Parse(Console.ReadLine());
                        Console.Write("\t\t Re-Enter payee CARD no: ");
                        int check_payee = int.Parse(Console.ReadLine());

                        if (payee_atm_no == check_payee)
                        {
                            if (usr[i].atm_no == check_payee)
                            {
                                Start.UI_error("You cannot your CARD no!");
                                return;
                            }

                            for (int x = 0; x < usr.Length; x++)
                            {
                                if (usr[x].atm_no == check_payee) // valid acc_no  
                                {
                                    flag = true;
                                    pae = x;
                                }
                            }

                            if (flag)
                            {
                                Console.Write("\t\t Enter amount : ");
                                double temp = int.Parse(Console.ReadLine());
                                if (temp >= 100)
                                {
                                    if (temp <= Bank.WITHDRAW_LIMIT)
                                    {
                                        usr[pae].bal += temp;
                                        record_trx(i, dt.ToString("d"), temp, "-TRNFR:" + payee_atm_no.ToString());
                                        usr[i].bal -= temp;
                                        record_trx(pae, dt.ToString("d"), temp, "+TRNFR:" + usr[i].atm_no.ToString());
                                        UI_loading();
                                    }
                                    else
                                    {
                                        Start.UI_error("Funds can't exceed Rs.1000\n\t\t\t in single transaction!");
                                        return;
                                    }
                                }
                                else
                                {
                                    Start.UI_error("Invalid Fund!");
                                    return;
                                }
                            }
                            else
                            {
                                Start.UI_error("CARD number doesn't exist!");
                                return;
                            }

                        }
                        else
                        {
                            Start.UI_error("Both CARD no doesn't match!");
                            return;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

            }

            public void mini_stmt(int atm_numb, bool choice)
            {
                int i = 0;
                bool check = false;
                for (int j = 0; j < usr.Length; j++)
                {
                    if (usr[j].atm_no == atm_numb) // valid acc_no  
                    {
                        check = true;
                        i = j;
                    }
                }

                if (check)
                {
                    Start.main_header();
                    Console.WriteLine("\t\t\t\tUser: " + usr[i].usrName);
                    Console.Write("\t\t" + ATM_LOC);
                    Console.WriteLine("\n\n\t\tDATE\t\tTIME\tATM ID \t");
                    Console.WriteLine("\t\t" + dt.ToString("d") + "\t" + dt.ToString("H:mmtt") + "\t" + ATM_ID);
                    Console.WriteLine("\t\tAcc No:" + usr[i].usrAcc_no + " " + "CARD No:00xxxx" + usr[i].atm_no);
                    Console.WriteLine("\t\t------------------------------------");
                    if (usr[i].Trx_count == 0 && choice)
                    {
                        Console.WriteLine("\n\t\t  No transactions recorded!\n");
                        Start.footer();
                        Console.Write("\t\tPress <any> key to continue:");
                        Console.ReadKey();
                        return;
                    }

                    if (choice)
                    {
                        Console.WriteLine("\t\t\t LAST 5 TRANSACTIONS");
                        for (int x = 0; x < usr[i].Trx_count; x++)
                        {
                            Console.WriteLine("\t\t" + usr[i].Trx_date[x] + "\tRs." + usr[i].Trx_amt[x] + "\t" + usr[i].Trx_mode[x]);
                        }
                    }
                    else
                    {
                        if (usr[i].Trx_mini_count == 0)
                        {
                            Console.WriteLine("\n\t\t  No transactions recorded!\n");
                            Start.footer();
                            Console.Write("\t\tPress <any> key to continue:");
                            Console.ReadKey();
                            return;
                        }
                        for (int x = 0; x < usr[i].Trx_mini_count; x++)
                        {
                            if (usr[i].Trx_mini_amt[x] == 0)
                            {
                                Console.WriteLine("\t\t" + usr[i].Trx_mini_date[x] + "\t" + usr[i].Trx_mini_mode[x]);
                            }
                            else
                            {
                                Console.WriteLine("\t\t" + usr[i].Trx_mini_date[x] + "\tRs." + usr[i].Trx_mini_amt[x] + "\t" + usr[i].Trx_mini_mode[x]);
                            }
                        }
                    }

                    Console.WriteLine("\n\t\t  AVAIL BAL: \tRs." + usr[i].bal);
                    Start.footer();
                    Console.Write("\t\tPress <any> key to continue:");
                    Console.ReadKey();
                }
            }

            public void chng_pin(int atm_numb)
            {
                int i = 0;
                bool check = false;
                for (int j = 0; j < usr.Length; j++)
                {
                    if (usr[j].atm_no == atm_numb) // valid acc_no  
                    {
                        check = true;
                        i = j;
                    }
                }

                if (check)
                {
                    Start.main_header();
                    Console.WriteLine("\t\t\t  User: " + usr[i].usrName + " \n\n");
                    Console.Write("\t\t Enter existing pin: ");
                    try
                    {
                        string old_pin = ""; ;
                        ConsoleKeyInfo key;
                        do
                        {
                            key = Console.ReadKey(true);

                            // Backspace Should Not Work  
                            if (key.Key != ConsoleKey.Backspace)
                            {
                                old_pin += key.KeyChar;
                                Console.Write("*");
                            }
                            else
                            {
                                if ((old_pin.Length - 1) != -1)
                                {
                                    old_pin = old_pin.Remove(old_pin.Length - 1);
                                    Console.Write("\b \b");
                                }
                            }
                        } // Stops Receiving Keys Once Enter is Pressed  
                        while (key.Key != ConsoleKey.Enter);

                        if (usr[i].usrPin == int.Parse(old_pin))
                        {
                            Console.Write("\n\t\t Enter new pin: ");
                            ConsoleKeyInfo key1;
                            old_pin = null;
                            do
                            {
                                key1 = Console.ReadKey(true);

                                // Backspace Should Not Work  
                                if (key1.Key != ConsoleKey.Backspace)
                                {
                                    old_pin += key1.KeyChar;
                                    Console.Write("*");
                                }
                                else
                                {
                                    if ((old_pin.Length - 1) != -1)
                                    {
                                        old_pin = old_pin.Remove(old_pin.Length - 1);
                                        Console.Write("\b \b");
                                    }
                                }
                            }// Stops Receiving key1s Once Enter is Pressed  
                            while (key1.Key != ConsoleKey.Enter);

                            Console.Write("\n\t\t Re-Enter new pin: ");
                            string cur_pin = ""; ;
                            do
                            {
                                key = Console.ReadKey(true);

                                // Backspace Should Not Work  
                                if (key.Key != ConsoleKey.Backspace)
                                {
                                    cur_pin += key.KeyChar;
                                    Console.Write("*");
                                }
                                else
                                {
                                    if ((cur_pin.Length - 1) != -1)
                                    {
                                        cur_pin = cur_pin.Remove(cur_pin.Length - 1);
                                        Console.Write("\b \b");
                                    }
                                }
                            }// Stops Receiving Keys Once Enter is Pressed  
                            while (key.Key != ConsoleKey.Enter);

                            if (int.Parse(old_pin) == int.Parse(cur_pin))
                            {
                                usr[i].usrPin = int.Parse(cur_pin);
                                record_mini_trx(i, dt.ToString("d"), 0, "CHANGE OF PIN");
                                UI_loading();
                            }
                            else
                            {
                                Start.UI_error("Both Pin doesn't match!");
                                return;
                            }
                        }
                        else
                        {
                            Start.UI_error("Incorrect Pin");
                            return;
                        }
                    }
                    catch (Exception)
                    {


                    }
                }
            }

            // for last 5 transaction recording  
            public void record_trx(int i, string date, double bal, string mode)
            {
                record_mini_trx(i, date, bal, mode);

                if (usr[i].Trx_count < 5)
                {
                    usr[i].Trx_date[(usr[i].Trx_count)] = date;
                    usr[i].Trx_amt[(usr[i].Trx_count)] = bal;
                    usr[i].Trx_mode[(usr[i].Trx_count)] = mode;
                    usr[i].Trx_count++;
                }
                else
                {

                    for (int k = 0; k < 4; k++)
                    {
                        usr[i].Trx_date[k] = usr[i].Trx_date[k + 1];
                        usr[i].Trx_amt[i] = usr[i].Trx_amt[k + 1];
                        usr[i].Trx_mode[i] = usr[i].Trx_mode[k + 1];
                    }
                    usr[i].Trx_date[4] = date;
                    usr[i].Trx_amt[4] = bal;
                    usr[i].Trx_mode[4] = mode;
                }
            }

            // for n no.of transaction recording  
            public void record_mini_trx(int i, string date, double bal, string mode)
            {
                usr[i].Trx_mini_date[(usr[i].Trx_mini_count)] = date;
                usr[i].Trx_mini_amt[(usr[i].Trx_mini_count)] = bal;
                usr[i].Trx_mini_mode[(usr[i].Trx_mini_count)] = mode;
                usr[i].Trx_mini_count++;
            }
        }


        class Start
        {
            public static string mt = "\t\t\t";
            public static string st = "\t\t  ";
            public static string xst = "\t\t";
            static int count, mnuChoice = 0;// count noof trials for login if > 3 exit user!!  

            //UI  
            public static void header()
            {
                Console.Clear();
                Console.WriteLine("\n\n\n\n\t\t====================================");
                Console.WriteLine(mt + "State Bank of Pandora");
                Console.WriteLine("\t\t====================================");
            }

            public static void main_header()
            {
                Console.Clear();
                Console.WriteLine("\n\n\t\t\t" + xst + ATM.dt.ToString("d") + "\n\t\t====================================");
                Console.WriteLine(mt + "State Bank of Pandora");
                Console.WriteLine("\t\t====================================");

            }

            public static void footer()
            {
                Console.WriteLine("\t\t====================================");
            }

            public static void UI_home()
            {
                header();
                Console.WriteLine("\n\n" + mt + "SBP ATM FACILTIY MAKES");
                Console.WriteLine(mt + "LIFE SIMPLE\n\n");
                footer();
                Console.Write(st + "Press <any> key to continue:");
                Console.ReadKey();

            }

            public static int UI_login(ref ATM u, ref int atm_no)
            {
                header();
                Console.Write(st + "Enter your CARD no: ");
                try
                {
                    bool check = false;
                    int i = 0;
                    int atm_numb = int.Parse(Console.ReadLine());
                    atm_no = atm_numb;


                    for (int j = 0; j < u.usr.Length; j++)
                    {
                        if (u.usr[j].atm_no == atm_numb) // valid acc_no  
                        {
                            check = true;
                            i = j;
                        }
                    }

                    if (check) // valid acc_no  
                    {
                        Console.Write(st + "Enter your PIN: ");
                        string acc_pin = ""; ;
                        ConsoleKeyInfo key;

                        do
                        {
                            key = Console.ReadKey(true);

                            // Backspace Should Not Work  
                            if (key.Key != ConsoleKey.Backspace)
                            {
                                acc_pin += key.KeyChar;
                                Console.Write("*");
                            }
                            else
                            {
                                if ((acc_pin.Length - 1) != -1)
                                {
                                    acc_pin = acc_pin.Remove(acc_pin.Length - 1);
                                    Console.Write("\b \b");
                                }
                            }
                        } // Stops Receving Keys Once Enter is Pressed  
                        while (key.Key != ConsoleKey.Enter);



                        if ((u.usr[i].usrPin == int.Parse(acc_pin)) && (u.usr[i].atm_no == atm_numb)) // valid acc_no && pass  
                        {
                            return 1; // 1: means successfull  
                        }
                        else
                        {
                            // Boot out usr if exceeded Trails == 3  
                            u.usr[i].usrMaxTrail = count++;

                            if (u.usr[i].usrMaxTrail >= 2)
                            {
                                count = 0;
                                UI_error("Reached Maxed Limit of Trying");
                                Main();
                            }
                            else
                            {
                                //< 3 trials but invalid password  
                                return 2;
                            }
                        }

                    }
                    else // Invalid account number  
                    { return 3; }


                }
                catch (Exception)
                {
                    return -1;
                }
                // defualt : false  
                return 0;
            }

            public static void UI_error(String error)
            {
                header();
                Console.WriteLine("\n\n" + st + " ERROR: " + error + "\n\n");
                footer();
                Console.Write(st + "Press <any> key to continue:");
                Console.ReadKey();

            }

            public static void UI_msgbox(String msg)
            {
                header();
                Console.WriteLine("\n\n" + st + msg + "\n\n");
                footer();
                Console.Write(st + "Press <any> key to continue:");
                Console.ReadKey();

            }

            public static int UI_main(ref int atm_n)
            {

                ATM validUsr = new ATM();
                string name = "";
                main_header();

                for (int i = 0; i < validUsr.usr.Length; i++)
                {
                    if (validUsr.usr[i].atm_no == atm_n)
                    {
                        name = validUsr.usr[i].usrName;

                    }
                }


                Console.WriteLine(xst + "\tWelcome: " + name + ".\n");
                Console.WriteLine(xst + "1.Deposit.\t\t2.Withdrawal.\n");
                Console.WriteLine(xst + "3.Balance INQ.\t\t4.Fund Trans.\n");
                Console.WriteLine(xst + "5.Mini Statement.\t6.Change Pin.\n");
                Console.WriteLine(xst + "7.Last 5 Transactions.\t8.Logout.\n");

                footer();
                Console.Write(st + "Enter your choice: ");
                try
                {
                    int ch = int.Parse(Console.ReadLine());
                    return ch;
                }
                catch (Exception)
                {
                    return -1;
                }

            }

            public static void Main()
            {
                ATM a = new ATM();

            SUDO_MAIN: // from MAIN MENUS logic // to avoid re-initilialize ATM class!!  
                int getAtm_no = 0;
                UI_home(); // Homepage  
                           // Login Page logic - Do not mess it up!!  
            LOGIN:
                int isValidLogin = UI_login(ref a, ref getAtm_no); // calls login UI which return integers   
                switch (isValidLogin)
                {
                    case -1:
                        UI_error("Alphabets Not Allowed");
                        goto LOGIN;
                    case 1:
                        goto MAIN_MENU; // Goto Main Menus if valid user  
                    case 2:
                        UI_error("Invalid Password");
                        goto LOGIN;
                    case 3:
                        UI_error("Invalid Account Number");
                        goto LOGIN;
                    default:
                        UI_error("Something Wrong in GUI_login()!");
                        Main();
                        break;
                }


            //MAIN MENUS logic  
            MAIN:
            MAIN_MENU: // from Login Page  
                while (true)
                {
                    mnuChoice = UI_main(ref getAtm_no); // call MAIN_UI  
                    switch (mnuChoice)
                    {
                        case -1:
                            UI_error("Alphabets Not Allowed");
                            goto MAIN;
                        case 1:
                            a.deposit(getAtm_no);
                            break;
                        case 2:
                            a.withdraw(getAtm_no);
                            break;
                        case 3:
                            a.balance(getAtm_no);
                            break;
                        case 4:
                            a.tnfr_fund(getAtm_no);
                            break;
                        case 5:
                            a.mini_stmt(getAtm_no, false); //false for full n no.of tranx recordings   
                            break;
                        case 6:
                            a.chng_pin(getAtm_no);
                            break;
                        case 7:
                            a.mini_stmt(getAtm_no, true); //true for last 5 noof tranx recordings   
                            break;
                        case 8:
                            UI_msgbox("Thankyou for visting SPB ATM.\n");
                            GC.SuppressFinalize(a);//Garbage Collection  
                            goto SUDO_MAIN;
                        default:
                            UI_error("Invalid Choice! [1-8].");
                            Main();
                            break;
                    }
                }
            }
        }
    }
    
    }

 

