using Q1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Q1
{
    /// <summary>
    /// Interaction logic for EmployeeWindown.xaml
    /// </summary>
    public partial class EmployeeWindown : Window
    {
        PRN221_TrialContext context = new PRN221_TrialContext();
        public EmployeeWindown()
        {
            InitializeComponent();
            getData();
        }
        public void getData()
        {
            var list = context.Employees.ToList();
            dgvEmployee.ItemsSource = list;
        }
        private void rbMale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtEmployeeId.Text = string.Empty;
            txtEmployeeName.Text = string.Empty;
            txtIdNumber.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtDOB.Text = string.Empty;
            rbMale.IsChecked = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Employee em = new Employee()
            {
                Name = txtEmployeeName.Text,
                Gender = rbMale.IsChecked == true ? "Male" : "Female",
                Dob = DateTime.Parse(txtDOB.Text),
                Phone = txtPhone.Text,
                Idnumber = txtIdNumber.Text
            };

            try
            {
                context.Employees.Add(em);
                context.SaveChanges();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            getData();

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Employee em = new Employee()
            {
                Id = int.Parse(txtEmployeeId.Text),
                Name = txtEmployeeName.Text,
                Gender = rbMale.IsChecked == true ? "Male" : "Female",
                Dob = DateTime.Parse(txtDOB.Text),
                Phone = txtPhone.Text,
                Idnumber = txtIdNumber.Text
            };

            Employee update = context.Employees.FirstOrDefault(x => x.Id == em.Id);
            update.Id = em.Id;
            update.Name = em.Name;
            update.Gender = em.Gender.ToString();
            update.Phone = em.Phone.ToString();
            update.Idnumber = em.Idnumber;

            try
            {
                context.Employees.Update(em);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            getData();

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            Employee remove = context.Employees.Where(x => x.Id == int.Parse(txtEmployeeId.Text)).FirstOrDefault();

            try
            {
                context.Employees.Remove(remove);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            getData();
        }

        private void dgvEmployee_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Employee selectedRow = (Employee)dgvEmployee.SelectedItem;
            if (selectedRow != null)
            {
               txtEmployeeId.Text = selectedRow.Id.ToString();
                txtEmployeeName.Text = selectedRow.Name.ToString();
                txtIdNumber.Text = selectedRow.Idnumber.ToString();
                txtPhone.Text = selectedRow.Phone.ToString();
                txtDOB.Text = selectedRow.Dob.ToString();
                rbMale.IsChecked = selectedRow.Gender.Equals("Male") ? true : false; ;
                rbFemale.IsChecked = selectedRow.Gender.Equals("Female") ? true : false;
            }
        }
    }
}
