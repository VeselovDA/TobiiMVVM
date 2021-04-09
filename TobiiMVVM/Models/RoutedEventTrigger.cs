using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace TobiiMVVM.Models
{
    public class RoutedEventTrigger : EventTriggerBase<DependencyObject>
    {
        #region Fields 

        /// <summary>
        /// The underlying RoutedEvent
        /// </summary>
        private RoutedEvent routedEvent;

        #endregion Fields 

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventTrigger"/> class.
        /// </summary>
        public RoutedEventTrigger()
        {
        }

        #region Properties 

        /// <summary>
        /// Gets or sets the underlying RoutedEvent.
        /// </summary>
        /// <value>The RoutedEvent.</value>
        public RoutedEvent RoutedEvent
        {
            get
            {
                return this.routedEvent;
            }

            set
            {
                this.routedEvent = value;
            }
        }

        #endregion Properties 

        #region Methods 

        /// <summary>
        /// Specifies the name of the Event this EventTriggerBase is listening for.
        /// </summary>
        /// <returns>The name of the event associated with this instance</returns>
        protected override string GetEventName()
        {
            // return RoutedEvent.Name;
            return "1";
        }

        /// <summary>
        /// Called after the trigger is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            Behavior behavior = this.AssociatedObject as Behavior;
            FrameworkElement associatedElement = this.AssociatedObject as FrameworkElement;
            if (behavior != null)
            {
                associatedElement = ((IAttachedObject)behavior).AssociatedObject as FrameworkElement;
            }

            if (associatedElement == null)
            {
                throw new ArgumentException("Routed Event trigger can only be associated to framework elements");
            }

            if (RoutedEvent != null)
            {
                associatedElement.AddHandler(RoutedEvent, new RoutedEventHandler(this.OnRoutedEvent));
            }
        }

        /// <summary>
        /// Called when the RoutedEvent fires
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnRoutedEvent(object sender, RoutedEventArgs args)
        {
            this.OnEvent(args);
        }

        #endregion Methods 
    }
}
