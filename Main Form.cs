using System.Windows.Forms.Design;

namespace Major_Project_1
{
    //Graham Reid, Major Project 1, 3/7/23. BUS 442//
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Declare all variables//

        int dailyOrderCount;
        double dailyOrderPrice;

        int quantity;
        double discount, discountAmount, totalAmount, itemAmount, subtotal, surcharges, tax, netOrder, taxCalculator, discountDisplay, averageTotalPerOrder;

        private void quantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
                return;
            }
        }

        private void removeAllFlavorButton_Click(object sender, EventArgs e)
        {
            flavorSelectionComboBox.Items.Clear();
        }

        private void removeFlavorButton_Click(object sender, EventArgs e)
        {
            //Remove selected item//
            
            flavorSelectionComboBox.Items.Remove(flavorSelectionComboBox.SelectedItem);

            int itemcount = flavorSelectionComboBox.Items.Count;
            totalFlavorsLabel.Text = itemcount.ToString("N0");
        }

        private void addFlavorButton_Click(object sender, EventArgs e)
        {
            //Add selected item//
            
            String newFlavor = flavorSelectionComboBox.Text;
            flavorSelectionComboBox.Items.Add(newFlavor);

            int itemcount = flavorSelectionComboBox.Items.Count;
            totalFlavorsLabel.Text = itemcount.ToString("N0");
        }

        private void summaryButton_Click(object sender, EventArgs e)
        {
            // Construct the summary message
            string message = "Total number of orders: " + dailyOrderCount.ToString() + Environment.NewLine +
                             "Daily order total: $" + dailyOrderPrice.ToString("0.00") + Environment.NewLine;
            if (dailyOrderCount > 0)
            {
                message += "Average total amount per order: $" + averageTotalPerOrder.ToString("0.00") + Environment.NewLine;
            }

            // Display the summary message box
            MessageBox.Show(message, "Daily Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void newOrderButton_Click(object sender, EventArgs e)
        {
            //Declare summary variables//
            double totalSales = 0;
            int totalOrders = 0;
            
            //Add current order totals to summary totals//
            totalSales += totalAmount;
            totalOrders++;

            //Reset current order//
            quantityTextBox.Text = "0";
            cappuccinoRadioButton.Checked = true;
            espressoRadioButton.Checked = false;
            latteRadioButton.Checked = false;
            poundRadioButton.Checked = false;
            halfRadioButton.Checked = false;
            freshBrewRadioButton.Checked = false;
            volDiscountCheckBox.Checked = false;
            surchargeCheckBox.Checked = false;
            itemAmountLabel.Text = "$0.00";
            subtotalLabel.Text = "$0.00";
            surchargesLabel.Text = "$0.00";
            taxLabel.Text = "$0.00";
            netOrderLabel.Text = "$0.00";
            discountLabel.Text = "$0.00";
            totalLabel.Text = "$0.00";

            // Disable Clear button and focus on Quantity textbox//
            clearButton.Enabled = false;
            quantityTextBox.Focus();

            //Reset discount info//
            discountPercentLabel.Visible = false;
            discountButton.Visible = false;
            discountTextBox.Visible= false;

            //Reset subtotal//
            subtotal = 0;
            surcharges= 0;

            //Calculate # of orders//
            dailyOrderCount += 1;

            //Calculate total costs//
            dailyOrderPrice += totalAmount;

            //Calculate average amount per order//
            if (dailyOrderCount > 0)
                averageTotalPerOrder = dailyOrderPrice / dailyOrderCount;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            //Clear upper labels//
            quantityTextBox.Clear();
            surchargeCheckBox.Checked = false;
            itemAmountLabel.Text = null;

            //Reset Radio Buttons//
            cappuccinoRadioButton.Checked = true;
            espressoRadioButton.Checked = false;
            latteRadioButton.Checked = false;
            poundRadioButton.Checked = false;
            halfRadioButton.Checked = false;
            freshBrewRadioButton.Checked = false;
        }

        private void volDiscountCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            discountPercentLabel.Visible = true;
            discountButton.Visible = true;
            discountTextBox.Visible = true;
            discountTextBox.Focus();
        }

        private void discountBotton_Click(object sender, EventArgs e)
        {
            const int discountCalculate = 100;

            //Parse discount//
            double.TryParse(discountTextBox.Text, out discount);

            //Calculate discount//
            discountAmount = discount / discountCalculate;
            discountDisplay = netOrder * discountAmount;
            totalAmount = netOrder - discountDisplay;

            //Display discount//
            discountLabel.Text = discountDisplay.ToString("C2");

            //Update Total//
            totalLabel.Text = totalAmount.ToString("C2");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            //Exit Confirmation//
            DialogResult dialog = MessageBox.Show("Are you sure you want to exit?", this.Text,
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //If answer is yes, close the app//
            if (dialog == DialogResult.Yes)
                this.Close();
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            //Declare needed variables and constants//
            const double CAPPUCCINO = 3.25;
            const double ESPRESSO = 3.50;
            const double LATTE = 3.00;
            const double ONEPOUND = 9.99;
            const double HALFPOUND = 5.99;
            const double FRESHBREW = 2.50;
            const double taxRate = 0.08;

            //Declare accumulator variable//
            double accumulator = 0.0;

            //Declare Surcharge Amount//
            double surchargePrice = 1.5;

            //Try to parse the quantity textbox//
            int.TryParse(quantityTextBox.Text, out quantity);

            if (quantity >= 10)
                volDiscountCheckBox.Checked = true;

            //Find selected menu items//

            if (cappuccinoRadioButton.Checked)
                itemAmount = quantity * CAPPUCCINO;
            else if (espressoRadioButton.Checked)
                itemAmount = quantity * ESPRESSO;
            else if (latteRadioButton.Checked)
                itemAmount = quantity * LATTE;
            else if (poundRadioButton.Checked)
                itemAmount = quantity * ONEPOUND;
            else if (halfRadioButton.Checked)
                itemAmount = quantity * HALFPOUND;
            else if (freshBrewRadioButton.Checked)
                itemAmount = quantity * FRESHBREW;

            //Conditional logic for additional syrup charges for fresh brew//

            double coffeeSyrupCharge = .5;
            if (freshBrewRadioButton.Checked)
                if (flavorSelectionComboBox.SelectedIndex > -1) itemAmount += quantity * coffeeSyrupCharge;
            if (syrupFlavorsListBox.SelectedIndex > -1) itemAmount += quantity * coffeeSyrupCharge;
            
            //Update subtotal with accumulator//
            accumulator += itemAmount;
            subtotal += accumulator;

            if (surchargeCheckBox.Checked && halfRadioButton.Checked)
                surcharges = surchargePrice * quantity;

            //Calculate tax//

            taxCalculator = subtotal + surcharges;
            tax = taxRate * taxCalculator;

            //Net Order//

            netOrder = tax + taxCalculator;

            //Total Cost//

            totalAmount = netOrder - discountDisplay;

            //Display the output//

            itemAmountLabel.Text = itemAmount.ToString("C2");
            subtotalLabel.Text = subtotal.ToString("C2");
            taxLabel.Text = tax.ToString("C2");
            surchargesLabel.Text = surcharges.ToString("C2");
            netOrderLabel.Text = netOrder.ToString("C2");
            discountLabel.Text = discountAmount.ToString("C2");
            totalLabel.Text = totalAmount.ToString("C2");

            //Show Clear Button//

            clearButton.Enabled = true;
        }
    }
}