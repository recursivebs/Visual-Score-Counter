using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualScoreCounter.VSCounter.Configuration {

    class CounterSettings {

        public virtual bool PercentageRingShowsNextColor { get; set; } = true;

        public virtual PercentageColorSettings PercentageColorSettings { get; set; } = new PercentageColorSettings();

    }

}
