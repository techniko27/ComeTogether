using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;

namespace ComeTogetherApp
{
    class EventCostCalculator
    {
        public static async Task<int> getPersonalCost(Event ev, string userID)
        {
            List<ToDo> todoList = new List<ToDo>();
            int costSumm = 0;

            //IReadOnlyCollection<Firebase.Database.FirebaseObject<string>> veranstaltung_ToDos = null;
            try
            {
                var veranstaltung_ToDos = await App.firebase.Child("Veranstaltung_ToDo").Child(ev.ID).OnceAsync<string>();
                foreach (var todoID in veranstaltung_ToDos)
                {
                    var todo = await App.firebase.Child("ToDos").OrderByKey().StartAt(todoID.Key).LimitToFirst(1).OnceAsync<ToDo>();
                    todoList.Add(todo.ElementAt(0).Object);
                }
                
                var benutzer_ToDos = await App.firebase.Child("Benutzer_ToDo").Child(App.GetUserID).Child(ev.ID).OnceAsync<User_ToDo>();
                foreach (var todoID in benutzer_ToDos)
                {
                    if (todoID.Object.isPaying.Equals("true"))
                    {
                        string todoid = todoID.Key;

                        foreach (ToDo todoInList in todoList)
                        {
                            if (todoInList.ID.Equals(todoID.Key))
                            {
                                costSumm += todoInList.Kosten;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return costSumm;
        }
    }
}
