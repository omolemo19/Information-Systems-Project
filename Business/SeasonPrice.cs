using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PhumlaKamnandiHotelSystem.Business;
namespace PhumlaKamnandi.Business
{
    public class SeasonPrice
    {
        public enum Season
        {
            LowSeason,
            MidSeason,
            HighSeason
        }

        Collection<Booking> LowSeason;
        Collection<Booking> MidSeason;
        Collection<Booking> HighSeason;
        Season season;



        public SeasonPrice()
        {

        }


        //}
        public decimal TotalFee(DateTime checkin)
        {

            decimal price = 0;
            DateTime lowcheckin = new DateTime(2021, 12, 01);
            DateTime midcheckin = new DateTime(2021, 12, 08);
            DateTime Highcheckin = new DateTime(2021, 12, 16);

            if (checkin.Date < midcheckin.Date)
            {
                price = 550;

            }
            else if ((checkin.Date >= midcheckin.Date) && (checkin.Date < Highcheckin.Date))
            {
                price = 750;

            }
            else if (checkin.Date >= Highcheckin.Date)
            {
                price = 995;

            }
            return price;


        }
    }
}




