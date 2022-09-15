using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TripSafe.Reposotories
{
    public class ValidRoute
    {
        public int Id { get; set; }
        public int busId { get; set; }
        public String busName { get; set; }
        public int bus_cap { get; set; }
        public int rem_vac { get; set; }
    }
    public class route_schedule
    {
        public int routeId { get; set; }
        public String routeName { get; set; }
        public int busId { get; set; }
        public int remVacancy { get; set; }
        public String busName { get; set; }
        public int terminalId { get; set; }
    }

    public class TicketRepository
    {
        private string constr;

        public TicketRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
        }
        public Object searchBus(int start_terminal, int end_terminal, int arrivalTime, int vacancy, int day)
        {
            List<ValidRoute> results = new List<ValidRoute>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"with route_schedule as(
	                                select route.Id, route.busId,(select name from bus where bus.Id=route.busId) as busName,
                                    (select capacity from bus where bus.Id=route.busId ) as bus_cap
                                    from route,schedule as S , schedule as T
                                    where S.routeId=route.Id and S.terminalId={start_terminal} and T.terminalId={end_terminal} and S.stoppageIndex< T.stoppageIndex
                                    and S.arrivalTime<={arrivalTime}
                                )
                                select * from route_schedule;";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlCommand newCommand = new MySqlCommand(query))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                results.Add(new ValidRoute
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),

                                    busId = Convert.ToInt32(sdr["busId"]),

                                    busName = sdr["busName"].ToString(),
                                    bus_cap = Convert.ToInt32(sdr["bus_cap"]),
                                    rem_vac = Convert.ToInt32(sdr["bus_cap"])
                                });
                            }
                        }
                        con.Close();
                    }

                }
            }
            List<Object> res = new List<object>();
            foreach (var data in results)
            {
                var info = (getBusList(data, day, vacancy, start_terminal));
                if (info != null) res.Add(info);
            }

            return res;
        }
        public Object getBusList(ValidRoute validRoute, int day, int minVacancy, int start_terminal)
        {
            List<Trip> trip = new List<Trip>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"select * from trip 
                                where trip.routeId={validRoute.Id} and trip.date={day};";
                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            trip.Add(new Trip
                            {
                                routeId = Convert.ToInt32(sdr["routeId"]),

                                Id = Convert.ToInt32(sdr["Id"]),
                                driverId = Convert.ToInt32(sdr["driverId"]),

                                date = Convert.ToInt32(sdr["date"])
                            });
                        }
                    }
                    con.Close();
                }

            }
            if (trip.Count == 0)
            {
                return validRoute;
            }
            else
            {
                bool ok = false;
                bool isValid = (isValidRoute(validRoute, minVacancy, day, start_terminal, trip[0].Id, ref ok));
                if (isValid) return validRoute;
                else return null;
            }
        }

        public bool isValidRoute(ValidRoute validRoute, int minVacancy, int date, int targetTerminal, int tripId, ref bool isValid)
        {
            int passengerCount = 0;
            Dictionary<int, int> existence = new Dictionary<int, int>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"select   schedule.terminalId ,schedule. routeId, schedule.arrivalTime,schedule. departureTime, schedule.stoppageIndex, 
                                (case 
		                                when (select sum(ticket.passengerCount) from ticket where ticket.endTerminal=schedule.terminalId) is null then 0
                                        else (select sum(ticket.passengerCount) from ticket where ticket.endTerminal=schedule.terminalId)
	                                end ) as unboard_cnt,
                                (case 
		                                when (select sum(ticket.passengerCount) from ticket where ticket.startTerminal=schedule.terminalId) is null then 0
                                        else (select sum(ticket.passengerCount) from ticket where ticket.startTerminal=schedule.terminalId)
	                                end ) as  board_cnt
                                 from trip,schedule
                                 where trip.routeId=schedule.routeId and trip.routeId={validRoute.Id} and trip.Id={tripId} and trip.date={date} order by stoppageIndex;";
                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var info = (new
                            {
                                terminalId = Convert.ToInt32(sdr["terminalId"]),
                                routeId = Convert.ToInt32(sdr["routeId"]),
                                arrivalTime = Convert.ToInt32(sdr["arrivalTime"]),
                                departureTime = Convert.ToInt32(sdr["departureTime"]),
                                stoppageIndex = Convert.ToInt32(sdr["stoppageIndex"]),
                                unboard_cnt = Convert.ToInt32(sdr["unboard_cnt"]),
                                board_cnt = Convert.ToInt32(sdr["board_cnt"])
                            });
                            if (!existence.ContainsKey(info.terminalId))
                            {
                                existence[info.terminalId] = 1;
                                if (info.terminalId != targetTerminal)
                                {
                                    existence[info.terminalId] = 1;
                                    passengerCount += info.board_cnt;
                                    passengerCount -= info.unboard_cnt;
                                }
                                else
                                {
                                    existence[info.terminalId] = 1;
                                    passengerCount += info.board_cnt;
                                    passengerCount -= info.unboard_cnt;
                                    isValid = validRoute.bus_cap - passengerCount >= minVacancy;
                                    validRoute.rem_vac = validRoute.bus_cap - passengerCount;
                                    return isValid;
                                }
                            }
                        }
                    }
                    con.Close();
                }

            }


            return false;

        }

        public Object getDailyBoardingUnboardingInfo(int date)
        {
            List<Object> data = new List<object>();

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"with ticket_time as(
	                                select ticket.Id,ticket. passengerCount,ticket. startTerminal,ticket. endTerminal,ticket. passengerId,ticket. tripId,
                                    trip.date as tripDate from trip, ticket 
                                    where ticket.tripId=trip.Id and trip.date={date}
                                ),
                                boardCount as(
	                                select terminal.Id, terminal.name, terminal.districtId,
                                (select name from district where district.Id=terminal.districtId) as districtName,
                                    (case 
		                                when (select sum(passengerCount) from ticket_time where terminal.Id=ticket_time.startTerminal) is null then 0
                                        else (select sum(passengerCount) from ticket_time where terminal.Id=ticket_time.startTerminal)
	                                end
                                    ) as boardCnt,
                                    (
                                    case 
		                                when (select sum(passengerCount)  from ticket_time where terminal.Id=ticket_time.endTerminal) is null then 0
                                        else (select sum(passengerCount) from ticket_time where terminal.Id=ticket_time.endTerminal)
	                                end
    
                                    ) as unBoardCnt
    
    
	                                from terminal
                                ) 
                                select * from boardCount;";

                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            data.Add(new
                            {


                                Id = Convert.ToInt32(sdr["Id"]),
                                name = sdr["name"].ToString(),
                                districtId = Convert.ToInt32(sdr["districtId"]),
                                districtName = sdr["districtName"].ToString(),
                                boardCnt = Convert.ToInt32(sdr["boardCnt"]),
                                unBoardCnt = Convert.ToInt32(sdr["unBoardCnt"])
                            });
                        }
                    }
                    con.Close();
                }


            }


            return data;
        }

        private int searchDriverId(int date)
        {
            List<int> employeeIds = new List<int>();
            string query = $@"select * from employee
                            where employeeType =2 and employee.Id not in
                            (select driverId from trip
                            where date={date});";
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            employeeIds.Add(Convert.ToInt32(sdr["Id"]));
                        }
                    }
                    con.Close();
                }

            }
            Random rnd = new Random();
            return employeeIds[rnd.Next(employeeIds.Count)];
        }

        private Trip findTrip(int routeId, int date)
        {
            string query = $@"select * from trip 
                                where trip.routeId={routeId} and trip.date={date};";
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            return (new Trip
                            {
                                routeId = Convert.ToInt32(sdr["routeId"]),

                                Id = Convert.ToInt32(sdr["Id"]),
                                driverId = Convert.ToInt32(sdr["driverId"]),

                                date = Convert.ToInt32(sdr["date"])
                            });
                        }
                    }
                    con.Close();
                }

            }
            return createNewTrip(new Trip
            {
                routeId = routeId,
                date = date,
                driverId = searchDriverId(date)
            });


        }

        private Trip createNewTrip(Trip newTrip)
        {
            string query = $@"INSERT INTO trip
                            (driverId,routeId, date)  VALUES
                            ( {newTrip.driverId},{newTrip.routeId}  ,  {newTrip.date}  );";
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    newCommand.ExecuteNonQuery();
                    con.Close();

                }
                using (MySqlCommand newCommand = new MySqlCommand(" (select max(Id) as Id from trip);"))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            newTrip.Id = Convert.ToInt32(sdr["Id"].ToString());
                            break;
                        }
                    }
                    con.Close();
                }

            }
            return newTrip;

        }
        private int searchPassenger(String phone, String name)
        {
            int passengerId = 0;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand newCommand = new MySqlCommand($@"select * from user where phoneNumber={phone};"))

                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            passengerId = Convert.ToInt32(sdr["Id"].ToString());
                        }
                    }
                    con.Close();
                }

            }
            if (passengerId == 0)
            {
                string query = $@"INSERT INTO  user ( phoneNumber,name )
                            VALUES (?1 ,?2);";


                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand newCommand = new MySqlCommand(query))
                    {
                        newCommand.Parameters.AddWithValue("?1", phone);
                        newCommand.Parameters.AddWithValue("?2", name);
                        newCommand.Connection = con;
                        con.Open();
                        newCommand.ExecuteNonQuery();
                        con.Close();

                    }
                    using (MySqlCommand newCommand = new MySqlCommand(" (select max(Id) as Id from user)"))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                passengerId = Convert.ToInt32(sdr["Id"].ToString());
                                break;
                            }
                        }
                        con.Close();
                    }

                }

            }

            return passengerId;
        }
        public Ticket createTicket(SearchQuery searchQuery)
        {

            Trip trip = this.findTrip(searchQuery.routeId, searchQuery.date);
            int passengerId = searchPassenger(searchQuery.phone, searchQuery.name);
            Ticket newTicket = new Ticket
            {
                passengerId = passengerId,
                tripId = trip.Id,
                passengerCount = searchQuery.seats,
                startTerminal = searchQuery.start_terminal,
                endTerminal = searchQuery.end_terminal,

            };
            string query = $@"INSERT INTO  ticket ( passengerCount,startTerminal,
                            endTerminal,passengerId,tripId )
                            VALUES
                            ( {searchQuery.seats } ,
                             {searchQuery.start_terminal  },
                                {searchQuery.end_terminal },{ passengerId },{trip.Id } );";
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    newCommand.ExecuteNonQuery();
                    con.Close();

                }
                using (MySqlCommand newCommand = new MySqlCommand(" (select max(Id) as Id from ticket)"))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            newTicket.Id = Convert.ToInt32(sdr["Id"].ToString());
                            break;
                        }
                    }
                    con.Close();
                }

            }

            return newTicket;
        }

        public List<Object> getTickets(String phone)
        {

            String query = $@"
                        select   passengerCount, 
                        (select name from terminal where terminal.Id= ticket.startTerminal) as start_from,
                        (select name from terminal where terminal.Id= ticket.endTerminal) as end_to,
                        (select date from trip where trip.Id=ticket.tripId) as travel_date

                        , endTerminal, 
                        (select name from user where user.Id=ticket.passengerId) as passengerName,
                        (select phoneNumber from user where user.Id=ticket.passengerId) as passengerPhone

                        , tripId from
                        ticket,user 
                        where ticket.passengerId=user.Id and user.phoneNumber={phone};";
            List<Object> tickets = new List<object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {

                    using (MySqlCommand newCommand = new MySqlCommand(query))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                tickets.Add(new
                                {
                                    passengerCount = Convert.ToInt32(sdr["passengerCount"].ToString()),
                                    start_from = sdr["start_from"].ToString(),
                                    end_to = sdr["end_to"].ToString(),
                                    travel_date = Convert.ToInt32(sdr["travel_date"].ToString()),

                                    passengerName = sdr["passengerName"].ToString(),
                                    passengerPhone = sdr["passengerPhone"].ToString(),

                                });
                            }
                        }
                        con.Close();
                    }
                }

            }
            return tickets;
        }

    
        public List<Object> getDailyTickets(int day)
        {
            String query = $@" select   passengerCount, 
                        (select name from terminal where terminal.Id= ticket.startTerminal) as start_from,
                        (select name from terminal where terminal.Id= ticket.endTerminal) as end_to,
                        (select date from trip where trip.Id=ticket.tripId) as travel_date,
                        (select name from user where user.Id=ticket.passengerId) as passengerName,
                        (select phoneNumber from user where user.Id=ticket.passengerId) as passengerPhone, tripId from
                        ticket,user ,trip
                        where ticket.passengerId=user.Id and trip.Id=ticket.tripId and trip.date={day};";
            List<Object> tickets = new List<object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query))
                {

                    using (MySqlCommand newCommand = new MySqlCommand(query))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                tickets.Add(new
                                {
                                    passengerCount = Convert.ToInt32(sdr["passengerCount"].ToString()),
                                    start_from = sdr["start_from"].ToString(),
                                    end_to = sdr["end_to"].ToString(),
                                    travel_date = Convert.ToInt32(sdr["travel_date"].ToString()),

                                    passengerName = sdr["passengerName"].ToString(),
                                    passengerPhone = sdr["passengerPhone"].ToString(),

                                });
                            }
                        }
                        con.Close();
                    }
                }

            }
            return tickets;
        }
    
    }
}