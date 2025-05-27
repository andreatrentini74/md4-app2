using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md4.Behaviors
{
    public class NumericRangeBehavior : Behavior<Entry>
    {
        public int Min { get; set; }
        public int Max { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Unfocused += OnUnfocused;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Unfocused -= OnUnfocused;
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;
            if (int.TryParse(entry.Text, out int value))
            {
                if (value < Min) value = Min;
                else if (value > Max) value = Max;

                entry.Text = value.ToString();
            }
            else
            {
                entry.Text = Min.ToString();
            }
        }
    }

}
