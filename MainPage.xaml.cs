/*Carlos Fernandez
27 / 11 / 2020
 CarSales



using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CarSalesAPP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        String[] phoneNo = new string[10];

        String[] name = new string[10];

        String[] make = new string[8];
        //private string[] phoneNo = { "04554933", "08280522", "084566995", "043366559", "047755881", "047531592", "04569871", "045648971", "04182354", "08456547" };
        //private string[] name = { "Carlos", "Adrian", "KT", "Anna", "Kris", "Paul", "Joshua", "Vani", "Peggy", "Eva" };
        //private string[] make = { "Toyota", "Holden", "Mitsubishi", "Ford", "BMW", "Mazda", "Volkswagen", "Mini" };

        const double WINDOW_TINTING = 150;
        const double DUCO_PROTECTION = 180;
        const double GPS = 320;
        const double SOUND = 350;


        public MainPage()
        {
            this.InitializeComponent();
        }
        private double calcOptionalExtras()
        {
            double windowCost = 0;
            double ducoCost = 0;
            double gpsCost = 0;
            double soundCost = 0;
            double optionalExtras;


            //  Calculate the optional extras
            if (windowTintingCheckBox.IsChecked == true)
            {
                windowCost = WINDOW_TINTING;
            }
            else
            {
                windowCost = 0;
            }
            if (ducoProtectionCheckBox.IsChecked == true)
            {
                ducoCost = DUCO_PROTECTION;
            }
            else
            {
                ducoCost = 0;
            }

            if (gpsNavigationCheckBox.IsChecked == true)
            {
                gpsCost = GPS;
            }
            else
            {
                gpsCost = 0;
            }
            if (sounSystemCheckBox.IsChecked == true)
            {
                soundCost = SOUND;
            }
            else
            {
                soundCost = 0;
            }
            optionalExtras = windowCost + ducoCost + gpsCost + soundCost;
            return optionalExtras;
        }

        //calculate the insurance
        private double calcAccidentInsurance(double vehiclePrice, double optionalExtras)
        {
            const double UNDER_AGE = 0.20;
            const double OVER_AGE = 0.10;

            double accidentInsuranceCost;

            if (underRadioButton.IsChecked == true)
            {

                accidentInsuranceCost = (double.Parse(vehiclePriceTextBox.Text) + optionalExtras) * UNDER_AGE;
            }
            else if (overRadioButton.IsChecked == true)
            {
                accidentInsuranceCost = (double.Parse(vehiclePriceTextBox.Text) + optionalExtras) * OVER_AGE;
            }
            else
            {
                accidentInsuranceCost = 0;
            }
            return accidentInsuranceCost;
        }

        //calculate vehicle warranty
        private double calcVehicleWarranty(double vehiclePrice)
        {
            const double COMM_RATE_0 = 0;
            const double COMM_RATE_1 = 0.05;
            const double COMM_RATE_2 = 0.10;
            const double COMM_RATE_3 = 0.20;
            double cost;

            //  Calculate the commission
            if (vehicleWarrantyComboBox.SelectedIndex == 0)
            {

                cost = double.Parse(vehiclePriceTextBox.Text) * COMM_RATE_0;
            }
            else if (vehicleWarrantyComboBox.SelectedIndex == 1)
            {
                cost = double.Parse(vehiclePriceTextBox.Text) * COMM_RATE_1;
            }
            else if (vehicleWarrantyComboBox.SelectedIndex == 2)
            {
                cost = double.Parse(vehiclePriceTextBox.Text) * COMM_RATE_2;
            }
            else if (vehicleWarrantyComboBox.SelectedIndex == 3)
            {
                cost = double.Parse(vehiclePriceTextBox.Text) * COMM_RATE_3;
            }
            else
            {
                cost = double.Parse(vehiclePriceTextBox.Text) * COMM_RATE_0;
            }
            return cost;
        }
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            phoneTextBox.Text = "";
            customerNameTextBox.Text = "";
            vehiclePriceTextBox.Text = "";
            tradeInTextBox.Text = "";
            subAmountTextBox.Text = "";
            gstAmountTextBox.Text = "";
            finalAmountTextBox.Text = "";
            summaryTextBlock.Text = "";
            insertVehicleMakeTextBox.Text = "";
            vehicleMakeTextBox.Text = "";



            phoneTextBox.IsEnabled = true;
            customerNameTextBox.IsEnabled = true;
            customerNameTextBox.Focus(FocusState.Programmatic);
            underRadioButton.IsEnabled = false;
            overRadioButton.IsEnabled = false;
            insuranceToggleSwitch.IsOn = false;
            windowTintingCheckBox.IsChecked = false;
            ducoProtectionCheckBox.IsChecked = false;
            gpsNavigationCheckBox.IsChecked = false;
            sounSystemCheckBox.IsChecked = false;
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (customerNameTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Customer Name cannot left blank");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }

            if (phoneTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Phone cannot left blank");
                await dialogMessage.ShowAsync();
                phoneTextBox.Focus(FocusState.Programmatic);
                phoneTextBox.SelectAll();
                return;
            }


            phoneTextBox.IsEnabled = false;
            customerNameTextBox.IsEnabled = false;
            vehiclePriceTextBox.Focus(FocusState.Programmatic);

        }

        private async void summaryButton_Click(object sender, RoutedEventArgs e)
        {
            const double GST_RATE = 0.1;
            double subAmount;
            double gstAmount;
            double finalAmount;
            double vehiclePrice;
            double lessTradeIn;
            double cost;
            double optionalExtras;
            double accidentInsuranceCost;

            try
            {
                vehiclePrice = double.Parse(vehiclePriceTextBox.Text);
            }
            catch (Exception theException)
            {
                var dialogMessage = new MessageDialog("Error! Please enter valid number " +
                    theException.Message);
                await dialogMessage.ShowAsync();
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
                return;
            }
            if (vehiclePrice <= 0)
            {
                var dialogMessage = new MessageDialog("Error! vehicle price has to be greater than 0 ");
                await dialogMessage.ShowAsync();
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
                vehiclePriceTextBox.SelectAll();
                return;
            }

            if (tradeInTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Traide In  is set to 0");
                await dialogMessage.ShowAsync();
                tradeInTextBox.Focus(FocusState.Programmatic);
                tradeInTextBox.SelectAll();
                tradeInTextBox.Text = "0";
                return;
            }

            try
            {
                lessTradeIn = double.Parse(tradeInTextBox.Text);
            }
            catch (Exception theException)
            {
                var dialogMessage = new MessageDialog("Error! Please enter a valid number " +
                    theException.Message);
                await dialogMessage.ShowAsync();
                tradeInTextBox.Focus(FocusState.Programmatic);
                return;
            }

            if (lessTradeIn < 0)    /*|| vehiclePrice <= lessTradeIn)*/
            {
                var dialogMessage = new MessageDialog("Error! vehicle price has to be equal or greater than 0  ");
                await dialogMessage.ShowAsync();
                tradeInTextBox.Focus(FocusState.Programmatic);
                tradeInTextBox.SelectAll();
                return;
            }

            if (vehiclePrice <= lessTradeIn)
            {
                var dialogMessage = new MessageDialog("Error! Traide In  has to be smaller than vehicle price");
                await dialogMessage.ShowAsync();
                tradeInTextBox.Focus(FocusState.Programmatic);
                tradeInTextBox.SelectAll();
                return;
            }

            summaryTextBlock.Text += "Customer Name:";
            summaryTextBlock.Text += customerNameTextBox.Text + "\n";
            summaryTextBlock.Text += "Phone:";
            summaryTextBlock.Text += phoneTextBox.Text + "\n";
            summaryTextBlock.Text += "Vehicle Price: $";
            summaryTextBlock.Text += vehiclePriceTextBox.Text + "\n";

            summaryTextBlock.Text += "Trade-In:$ ";
            summaryTextBlock.Text += tradeInTextBox.Text + "\n";

            cost = calcVehicleWarranty(vehicleWarrantyComboBox.SelectedIndex);
            summaryTextBlock.Text += "The warranty cost is : " + cost.ToString("C") + "\n";

            optionalExtras = calcOptionalExtras();
            summaryTextBlock.Text += "The optional extras cost is : " + optionalExtras.ToString("C") + "\n";

            accidentInsuranceCost = calcAccidentInsurance(vehiclePrice, optionalExtras);
            summaryTextBlock.Text += "The accident insurance cost is : " + accidentInsuranceCost.ToString("C") + "\n";

            subAmount = double.Parse(vehiclePriceTextBox.Text) + accidentInsuranceCost + cost + optionalExtras - double.Parse(tradeInTextBox.Text);
            subAmountTextBox.Text = subAmount.ToString("C");

            gstAmount = subAmount * GST_RATE;
            gstAmountTextBox.Text = gstAmount.ToString("C");

            finalAmount = subAmount + gstAmount;
            finalAmountTextBox.Text = finalAmount.ToString("C");

            summaryTextBlock.Text += "Final Amount:";
            summaryTextBlock.Text += finalAmountTextBox.Text + "\n";

        }


        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            {
                ToggleSwitch toggleSwitch = sender as ToggleSwitch;
                if (toggleSwitch != null)
                {
                    if (toggleSwitch.IsOn == true)
                    {
                        underRadioButton.IsEnabled = true;
                        underRadioButton.IsChecked = true;
                        overRadioButton.IsEnabled = true;
                    }
                    else
                    {
                        underRadioButton.IsEnabled = false;
                        overRadioButton.IsEnabled = false;
                    }

                }
            }
        }

        private void displayAllCustomers_Click(object sender, RoutedEventArgs e)
        {
            string output = "";
            for (int index = 0; index < name.Length; index++)  // loop through the array
            {
                output = output + name[index] + ", " + phoneNo[index] + "\n";  // display pops in ListBox
            }
            summaryTextBlock.Text = " Customer Name and Phone :\n" + output;
        }

        private int searchArray(string criteria)
        {
            int counter = 0;
            bool found = false;
            while (!found && counter < name.Length) // while not found and not end of array
            {
                if (criteria == name[counter].ToUpper())
                    found = true;
                else
                    counter++;      // if no match move to next element in array
            }
            if (counter < name.Length)
                return counter; //return index of array elemnt found
            else
                return -1; //retunr -1 if not found
        }

        private async void searchName_Click(object sender, RoutedEventArgs e)
        {
            displayAllCustomers_Click(sender, e);
            int counter = 0;                // track position in array
          
            if (customerNameTextBox.Text == "") //check if search textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a Customer Name into the Customer Name box");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                return;
            }

            string criteria = customerNameTextBox.Text.ToUpper();// input search criteria from user and convert to uppercase

            counter = searchArray(criteria);
            
            
            if (counter == -1)    // if found 
            {
                summaryTextBlock.Text = criteria + " not found ";     
            }
            else
            {
                                 
                phoneTextBox.Text = phoneNo[counter];                            
            }
        }

        private async void deleteName_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;				// used to keep track of position in array
            bool found = false;             // boolean to keep track if name found


            string criteria = customerNameTextBox.Text.ToUpper(); // input search criteria from user
            // do sequential search of string array until match found or end of array reached
            while (!found && counter < name.Length)	// while not found and not end of array
            {
                if (criteria == name[counter].ToUpper())	// case sensitive
                    found = true;
                else
                    counter++;		// if no match move to next element in array
            }
            if (counter < name.Length)    // if found the name exists, delete from the array
            {
                criteria = name[counter].ToUpper() + " with " + phoneNo[counter].ToString(); // adds the phone no of the customer to the criteria  
                for (int i = counter; i < name.Length - 1; i++)
                {
                    name[i] = name[i + 1];//copy the next item in the array to the previous position
                    phoneNo[i] = phoneNo[i + 1];//do the same for the corresponding pop array
                }
              
                Array.Resize(ref name, name.Length - 1);  // RESIZE name Array by -1 
                Array.Resize(ref phoneNo, phoneNo.Length - 1);
                displayAllCustomers_Click(sender, e); 
                
                var dialogMessage = new MessageDialog("Customer " + criteria + " has been deleted,\n customer list now updated to length " + name.Length); 
                await dialogMessage.ShowAsync(); 
            }
            else
            {
                summaryTextBlock.Text = criteria + " DOES NOT EXIST to delete";
            }

        }

        private void displayMakes_Click(object sender, RoutedEventArgs e)
        {     Array.Sort(make);                                             
            string output = "";
            for (int index = 0; index < make.Length; index++)  // loop through the array
            {
                output = output + make[index] + "\n";  // display pops in ListBox
            }
            summaryTextBlock.Text = " Vehicle Makes :\n" + output;
            
        }

        public static int arrayStringBinarySearch(string[] data, string item)
        {
            int min = 0;
            int max = data.Length - 1;
            int mid;
            item = item.ToUpper();
            do
            {
                mid = (min + max) / 2;
                if (data[mid].ToUpper() == item)  //if the item is found return the index mid
                                                  // or use if (item.CompareTo(data[mid].ToUpper()) == 0) 
                    return mid;
                if (item.CompareTo(data[mid].ToUpper()) > 0)   //check if the item wanted is in the top half of the search 
                    min = mid + 1;      //set the min part of the search to the mid +1
                else
                    max = mid - 1;      //otherwise the item must be in the lower half of the search, set max to the mid-1

            } while (min <= max);
            return -1;

        }


        private async void binarySearch_Click(object sender, RoutedEventArgs e)
        {
           
            string criteria;
            criteria = vehicleMakeTextBox.Text.ToUpper();
            Array.Sort(make);
            int foundPos = arrayStringBinarySearch(make, criteria); //call the arrayBinarySearch method
            if (vehicleMakeTextBox.Text == "") //check if search textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a vehicle make into the search box");
                await dialogMessage.ShowAsync();
                vehicleMakeTextBox.Focus(FocusState.Programmatic);
                return;
            }

            if (foundPos == -1)
            {
                var dialogMessage = new MessageDialog(criteria + " not found ");
                await dialogMessage.ShowAsync();
            }
            else
            {

                
                var dialogMessage = new MessageDialog(criteria + " found at index " + foundPos);
                await dialogMessage.ShowAsync();
            }

        }

        private async void insertVehicleMake_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;				// used to keep track of position in array
            bool found = false;				// boolean to keep track if make found
            if (insertVehicleMakeTextBox.Text == "") //check if search textbox empty
            {
                var dialogMessage = new MessageDialog("Please enter a new vehicle make into the insert box");
                await dialogMessage.ShowAsync();
                insertVehicleMakeTextBox.Focus(FocusState.Programmatic);
                return;
            }

            string criteria = insertVehicleMakeTextBox.Text.ToUpper();	// input search criteria from user
            // do sequential search of string array until match found or end of array reached
            while (!found && counter < make.Length)	// while not found and not end of array
            {
                if (criteria == make[counter].ToUpper())	
                    found = true;
                else
                    counter++;		// if no match move to next element in array
            }
            if (counter < make.Length)    // if found the make already exists, do not add to array
            {
                summaryTextBlock.Text = criteria + " ALREADY EXISTS";
            }
            else
            {
                Array.Resize(ref make, make.Length + 1);            // RESIZE make Array by 1
                var dialogMessage = new MessageDialog("make array now updated to length " + make.Length);
                await dialogMessage.ShowAsync();
                make[make.Length - 1] = insertVehicleMakeTextBox.Text;  // assign the make entered to the last element in the array
                displayMakes_Click(sender, e);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
             phoneNo = new string[] { "04554933", "08280522", "084566995", "043366559", "047755881", "047531592", "04569871", "045648971", "04182354", "08456547" };
         name = new string[] { "Carlos", "Adrian", "KT", "Anna", "Kris", "Paul", "Joshua", "Vani", "Peggy", "Eva" };
         make = new string[] { "Toyota", "Holden", "Mitsubishi", "Ford", "BMW", "Mazda", "Volkswagen", "Mini" };
    }
    }
}
    
