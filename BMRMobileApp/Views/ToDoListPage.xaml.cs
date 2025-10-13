using BMRMobileApp.Utilites;
using BMRMobileApp.ViewModels;
using Microsoft.Graph;
using Microsoft.VisualBasic;
//https://help.syncfusion.com/cr/maui-toolkit/Syncfusion.Maui.Toolkit.Calendar.SfCalendar.html?tabs=tabid-49%2Ctabid-31%2Ctabid-29%2Ctabid-5%2Ctabid-11%2Ctabid-17%2Ctabid-39%2Ctabid-1%2Ctabid-9%2Ctabid-7%2Ctabid-13%2Ctabid-3%2Ctabid-43%2Ctabid-21%2Ctabid-25%2Ctabid-23%2Ctabid-35%2Ctabid-27%2Ctabid-15%2Ctabid-47%2Ctabid-41%2Ctabid-37%2Ctabid-33%2Ctabid-19
namespace BMRMobileApp;

public partial class ToDoListPage : ContentPage
{
   public ToDoListViewModel toList = new ToDoListViewModel();

    private readonly RandomBackGroundColor backGroundColor = new RandomBackGroundColor();
    public ToDoListPage()
	{
		InitializeComponent();
		BindingContext = toList;
        toList.TaskDescriptionFocused = true;
         


    }
    protected override void OnAppearing()
    { 
    //MainThread.BeginInvokeOnMainThread(() =>
    //    {
    //       Shell.SetTabBarIsVisible(this, true);
    //});
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        toList?.StopVoiceJournalAsync();
        toList?.UnRegisterEvents();

    }
   
   



    private async void SfCalendar_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.Calendar.CalendarSelectionChangedEventArgs e)
    {
        var dt = DateTime.Parse(e.NewValue.ToString());
      
        toList.SelectedDate = dt.ToString("MM/dd/yyyy");
        toList.TaskDueDate = $"Date Task Due {toList.SelectedDate}  {toList.SelectedTime}";
        DueDateBC.BackgroundColor = await backGroundColor.RandomColor();
       // await toList.ChangeColorDueDate();
    }

    private void TaskNameFocous_TextChanged(object sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;
    }

    private void TaskNameFocous_Focused(object sender, FocusEventArgs e)
    {


       


    }

    private void TaskNameFocous_Unfocused(object sender, FocusEventArgs e)
    {
    }

    private void TimePicker_TimeSelected(object sender, TimeChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(toList.SelectedDate))
            toList.SelectedDate = DateTime.Now.ToString("MM-dd-yyyy");
            
        toList.SelectedTime = e.NewTime.ToString();
        toList.TaskDueDate = $"Date Task Due {toList.SelectedDate}  {toList.SelectedTime}";
    }
    

}