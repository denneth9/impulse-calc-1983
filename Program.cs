using System;
using System.Runtime.InteropServices;
using DustInTheWind.ConsoleTools; //import my saviour
using DustInTheWind.ConsoleTools.InputControls;
using DustInTheWind.ConsoleTools.TabularData;

namespace impulse_calc_1983
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Impulse Calculator 1983!"); //full disclosure, not actually made in 1983
            Console.WriteLine("Developed by Denneth Ahles");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(); //wait for keypress
            Console.Clear(); //clear screen
            Console.WriteLine("Please enter time interval between readings in milliseconds");
            Console.Write("> ");
            double timeinterval;
            //double timeinterval = (double) Console.Read() / 1000;
            try //try to get timeinterval
            {
                timeinterval = Convert.ToDouble(Console.ReadLine()) / 1000; //get input and divide by 1000 to get seconds from milliseconds
                Console.WriteLine("Time Interval (Seconds): " + timeinterval); //print it for sanity checking
            }
            catch (Exception e) //if a error occured, wait for keypress and crash
            {
                Console.WriteLine("Error with Input, press any key to exit"); 
                Console.ReadKey();
                throw;
                //Console.WriteLine(e);
            }

            //Console.WriteLine("Do you want to do specific impulse calculations? this requires propellant weight (Y/N)");
            double propellantweight = 0;
            //bool specificimpulse = false;
            bool specificimpulsedone = false;
            //attempt at button detection, to be removed
            /*do
            {
                /*
                Console.Write("> ");
                string consoletemp = Console.Read().ToString();
                if (consoletemp == "Y")
                {
                    Console.WriteLine("Please enter propellant weight in kilograms");
                    Console.Write("> ");
                    specificimpulse = true;
                    specificimpulsedone = true;
                    propellantweight = (double)Console.Read();
                    //return;
                }
                else if (consoletemp == "N")
                {
                    specificimpulsedone = true;
                    //return;
                }
                ConsoleKey response;
                do
                {
                    response = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
                    if (response != ConsoleKey.Enter)
                        Console.WriteLine("");
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                if (response == ConsoleKey.Y)
                {
                    //Console.WriteLine("yes");
                    do
                    {
                        Console.WriteLine("Please enter propellant weight in kilograms");
                        Console.Write("> ");
                        propellantweight = (double) Console.Read();
                        specificimpulse = true;
                    } while (propellantweight == 0);
                    specificimpulsedone = true;
                }
                else
                {
                    //Console.WriteLine("no");
                    specificimpulsedone = true;
                }

            } while (!specificimpulsedone);*/
        YesNoQuestion yesNoQuestion = new YesNoQuestion("Do you want to do specific impulse calculations? this requires propellant weight"); //use the sacred library to get a y/n input
        YesNoAnswer answerspi = yesNoQuestion.ReadAnswer();
        if (answerspi == YesNoAnswer.Yes) //if yes, prompt for weight
        {

                Console.WriteLine("Please enter propellant weight in kilograms");
                Console.Write("> ");
                //string i = Console.ReadLine();
                //Console.WriteLine(i);
                try
                {
                    propellantweight = Convert.ToDouble(Console.ReadLine());
                }
                catch (Exception e) //if a invalid input occurs, crash
                {
                    //Console.WriteLine(e);
                    Console.WriteLine("Error with Input, press any key to exit");
                    Console.ReadKey();
                    throw;
                }

                //specificimpulsedone = true;


        }
            Console.WriteLine("Please enter readings in kilograms one at a time, enter 'done' to continue");
            double maxreading = 0;
            double readingsum = 0;
            int readings = 0;
            double g = 9.80665;
            bool done = false;
            do //while done is false, prompt for readings
            {
                Console.Write("> ");
                string tmpreading = Console.ReadLine();
                if (tmpreading == "done")
                {
                    done = true;
                }
                else
                {
                    try
                    {
                        readingsum += Convert.ToDouble(tmpreading);
                        if (Convert.ToDouble(tmpreading) > maxreading)
                        {
                            maxreading = Convert.ToDouble(tmpreading);
                        }
                        readings++;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error Parsing Input, Ignored");
                        //throw;
                    }

                }
            } while (!done);

            /*
            while (done == false)
            {
                Console.Write("> ");
                if (Console.Read().ToString() == "done")
                {
                    done = true;
                }
                else
                {
                    double reading = (double) Console.Read();
                    readingsum += reading;
                    readings++;
                }
            }
            */
            Console.Clear();
            DataGrid dataGrid = new DataGrid("Impulse Data");
            double impulse = readingsum * timeinterval * g;
            double totaltime = readings * timeinterval;
            double averageimpulse = impulse / totaltime;
            //dataGrid.Columns.Add("Thrust Time");
            //dataGrid.Columns.Add("Impulse (Ns)");
            //dataGrid.Columns.Add("Average Impulse (Ns)");
            dataGrid.Rows.Add("Thrust Time", totaltime);
            dataGrid.Rows.Add("Peak Thrust (kg)", maxreading);
            dataGrid.Rows.Add("Impulse (Ns)", impulse);
            dataGrid.Rows.Add("Average Impulse (Ns)", averageimpulse);
            //dataGrid.AddRow("Thrust Time", totaltime);
            //Console.WriteLine("Total Thrust Time (Seconds): " + totaltime);
            //Console.WriteLine("Impulse (Ns): " + impulse);
            //Console.WriteLine("Average Impulse (Ns): " + averageimpulse);
            if (answerspi == YesNoAnswer.Yes)
            {
                double specificimpulse = impulse / propellantweight;
                dataGrid.Rows.Add("Specific Impulse (Ns/kg)", specificimpulse);
                dataGrid.Rows.Add("Specific Impulse (Sec)", specificimpulse / g);
                //dataGrid.Columns.Add("Specific Impulse (Ns/kg");
                //Console.WriteLine("Specific Impulse (Ns / kg): " + specificimpulse);
            }
            dataGrid.DisplayBorderBetweenRows = true;
            dataGrid.DisplayColumnHeaders = true;

            dataGrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;

            dataGrid.Display();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
