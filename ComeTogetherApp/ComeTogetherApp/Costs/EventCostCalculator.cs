using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    class EventCostCalculator
    {
        public static async Task<int> getPersonalCost(Event ev, string userID, Label costLabel, Button costButton)
        {
            //List<ToDo> todoList = new List<ToDo>();
            int personalCost = 0;
            int isPayingCount = 0;

            //IReadOnlyCollection<Firebase.Database.FirebaseObject<string>> veranstaltung_ToDos = null;
            try
            {
                /*
                var veranstaltung_ToDos = await App.firebase.Child("Veranstaltung_ToDo").Child(ev.ID).OnceAsync<string>();
                foreach (var todoID in veranstaltung_ToDos)
                {
                    var todo = await App.firebase.Child("ToDos").OrderByKey().StartAt(todoID.Key).LimitToFirst(1).OnceAsync<ToDo>();
                    todoList.Add(todo.ElementAt(0).Object);
                }
                */

                var benutzer_ToDos = await App.firebase.Child("Benutzer_ToDo").Child(userID).Child(ev.ID).OnceAsync<User_ToDo>();
                foreach (var todoID in benutzer_ToDos)
                {
                    var todoCollection = await App.firebase.Child("ToDos").OrderByKey().StartAt(todoID.Key).LimitToFirst(1).OnceAsync<ToDo>();
                    ToDo todo = todoCollection.ElementAt(0).Object;

                    if (todo.OrganisatorID.Equals(userID))
                    {
                        personalCost -= todo.Kosten;
                    }

                    if (todoID.Object.isPaying.Equals("true"))
                    {
                        var todo_Benutzer = await App.firebase.Child("ToDo_Benutzer").Child(todoID.Key).OnceAsync<string>();
                        personalCost += todo.Kosten / todo_Benutzer.Count;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                costLabel.Text = "Personal Cost: " + personalCost + "€";
            });

            Device.BeginInvokeOnMainThread(async () =>
            {
                costButton.Text = personalCost + "€";
            });

            return personalCost;
        }
    }
}
