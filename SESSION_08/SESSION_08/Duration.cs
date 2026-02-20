using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_08
{
    internal class Duration
    {
            public int hours ;
            public int minutes ;
            public int seconds ;
    
            public Duration(int hours, int minutes, int seconds)
            {
                this.hours = hours;
                this.minutes = minutes;
                this.seconds = seconds;
            }
    

            public Duration(int seconds) : this(seconds / 3600, (seconds % 3600) / 60, seconds  % 60) { }
            public Duration() : this(0, 0, 0) { }
    
            public override string ToString()
            {
                if(hours > 0)
                    return $"Hours: {hours}, Minutes :{minutes}, Seconds :{seconds}";
                else if(minutes > 0)
                    return $"Minutes :{minutes}, Seconds :{seconds}";
                else
                    return $"Seconds :{seconds}";
            }

        public override int GetHashCode() 
        { 
            return hours*3600 + minutes*60 + seconds ;
        }
        public override bool Equals(object obj)
        {
            if (obj is Duration D)
            {
                return this.GetHashCode() == D.GetHashCode();
            }
            return false;
        }
        // Equals is comparing the actual values

    }
}
