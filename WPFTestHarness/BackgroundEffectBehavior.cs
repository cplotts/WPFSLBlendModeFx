using Microsoft.Xaml.Behaviors;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace WPFTestHarness
{
	/// <summary>
	/// Custom behavior which allows you to apply a shader effect to the background of a Visual,
	/// instead of to the Visual itself. This behavior is extremely helpful when using blend modes
	/// to blend an object into its background.
	/// 
	/// This behavior was created by Jeremiah Morrill and was originally called GlassBehavior.
	/// http://jmorrill.hjtcentral.com/Home/tabid/428/EntryId/403/Glass-Behavior-for-WPF.aspx
	/// </summary>
	public class BackgroundEffectBehavior : Behavior<FrameworkElement>
	{
		#region Public Constructors
		/// <summary>
		/// Creates a new instance of the GlassBehavior
		/// </summary>
		public BackgroundEffectBehavior()
		{
			// Let's setup some possible optimizations
			RenderOptions.SetEdgeMode(m_attachedObjectVisualBrush, EdgeMode.Aliased);
			RenderOptions.SetCachingHint(m_attachedObjectVisualBrush, CachingHint.Cache);

			RenderOptions.SetEdgeMode(m_backgroundVisualBrush, EdgeMode.Aliased);
			RenderOptions.SetCachingHint(m_backgroundVisualBrush, CachingHint.Cache);

			// This makes sure our brush is not stretched.
			// This would make the glass not look correct relative to the visual it is displaying.
			m_attachedObjectVisualBrush.Stretch = Stretch.None;

			// The ViewboxUnits are absolute because our transformation values will be in absolute,
			// so this makes things easier.
			m_backgroundVisualBrush.ViewboxUnits = BrushMappingMode.Absolute;
			m_backgroundVisualBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
			m_backgroundVisualBrush.Viewport = new Rect(0, 0, 1, 1);
		}
		#endregion

		#region Public Dependency Properties
		#region Visual
		/// <summary>
		/// The target Visual to use for the background
		/// </summary>
		public Visual Visual
		{
			get { return (Visual)GetValue(VisualProperty); }
			set { SetValue(VisualProperty, value); }
		}
		public static readonly DependencyProperty VisualProperty =
			DependencyProperty.Register
			(
				"Visual",
				typeof(Visual),
				typeof(BackgroundEffectBehavior),
				new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnVisualChanged))
			);
		private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BackgroundEffectBehavior)d).OnVisualChanged(e);
		}
		protected virtual void OnVisualChanged(DependencyPropertyChangedEventArgs e)
		{
			SetupVisual();
		}
		#endregion

		#region Effect
		/// <summary>
		/// The pixel shader effect to apply to the background
		/// </summary>
		public Effect Effect
		{
			get { return (Effect)GetValue(EffectProperty); }
			set { SetValue(EffectProperty, value); }
		}
		public static readonly DependencyProperty EffectProperty =
			DependencyProperty.Register
			(
				"Effect",
				typeof(Effect),
				typeof(BackgroundEffectBehavior),
				new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnEffectChanged))
			);
		private static void OnEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BackgroundEffectBehavior)d).OnEffectChanged(e);
		}
		protected virtual void OnEffectChanged(DependencyPropertyChangedEventArgs e)
		{
			SetEffect();
		}
		#endregion
		#endregion

		#region Protected Overridden Methods
		/// <summary>
		/// Called when the behavior is attached to a DependencyObject
		/// </summary>
		protected override void OnAttached()
		{
			if (m_attachedObject != null)
			{
				// Unhook our old event and avoid any hidden refs.
				m_attachedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
			}

			m_attachedObject = AssociatedObject;

			// Search for a property we can set our VisualBrush to.
			// Right now we search for a Background or Fill property.
			PropertyInfo info = FindFillProperty(m_attachedObject);
			if (info != null)
			{
				info.SetValue(m_attachedObject, m_attachedObjectVisualBrush, null);
			}

			// Hook into the LayoutUpdated so we can keep everything in sync when the layout changes.
			m_attachedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;

			// Make sure our Visual is setup.
			SetupVisual();

			base.OnAttached();
		}

		/// <summary>
		/// Called when the behavior is removed from the DependencyObject
		/// </summary>
		protected override void OnDetaching()
		{
			if (m_attachedObject != null)
			{
				// Remove our handler to avoid any leaks.
				m_attachedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
			}

			base.OnDetaching();
		}
		#endregion

		#region Private Event Handlers
		private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
		{
			EnsureBrushSyncWithVisual();
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Hooks up the Visual so that it is used as the background on which the effect is applied.
		/// </summary>
		private void SetupVisual()
		{
			var element = Visual as FrameworkElement;

			if (element == null || m_attachedObject == null)
				return;

			// Set our pixel shader, if any.
			SetEffect();

			// 1. Set the background VisualBrush's Visual to the passed in Visual (which should be the background of the attached object).
			// 2. Fill the background Visual (upon which the Effect is applied) with the background VisualBrush (set in Step 1).
			//    Now, our effect has a background brush for AInput.
			m_backgroundVisualBrush.Visual = element;
			m_backgroundVisual.Fill = m_backgroundVisualBrush;

			// 3. Set the attached object VisualBrush's Visual to the background Visual (set in Step 2) on which the Effect is applied.
			//    This causes the attached object to look as if it has been blended into the background with the blend mode effect
			//    that has been applied.
			m_attachedObjectVisualBrush.Visual = m_backgroundVisual;

			EnsureBrushSyncWithVisual();
		}

		/// <summary>
		/// Applies the passed in Effect to the background visual.
		/// </summary>
		private void SetEffect()
		{
			if (m_backgroundVisual == null)
				return;

			m_backgroundVisual.Effect = Effect;
		}

		/// <summary>
		/// Keeps the background VisualBrush synced up with the passed in Visual, utilizing the Viewbox to do so,
		/// as the attached object could be moving around on top of the passed in Visual.
		/// </summary>
		private void EnsureBrushSyncWithVisual()
		{
			if (m_attachedObject == null || Visual == null)
				return;

			// Make the background visual the same size of our attached FrameworkElement.
			m_backgroundVisual.Width = m_attachedObject.ActualWidth;
			m_backgroundVisual.Height = m_attachedObject.ActualHeight;

			// Get the transform of our attached FrameworkElement to the Visual we want to use as our background.
			GeneralTransform trans = m_attachedObject.TransformToVisual(Visual);

			// Calculate the difference between 0,0 coord of our attached FrameworkElement
			// and 0,0 coord of our target Visual for the background.
			Point pos = trans.Transform(new Point(0, 0));

			// Create a new Viewbox for the VisualBrush. This shows a specific area of the Visual.
			var viewbox = new Rect
			{
				X = pos.X,
				Y = pos.Y,
				Width = m_attachedObject.ActualWidth,
				Height = m_attachedObject.ActualHeight
			};

			m_backgroundVisualBrush.Viewbox = viewbox;
		}

		/// <summary>
		/// Searches for a property on DependencyObject to set a Brush to.
		/// </summary>
		/// <param name="obj">The DependencyObject to search</param>
		/// <returns></returns>
		private static PropertyInfo FindFillProperty(DependencyObject obj)
		{
			Type t = obj.GetType();

			PropertyInfo info = t.GetProperty("Background") ?? t.GetProperty("Fill");

			return info;
		}
		#endregion

		#region Private Fields
		/// <summary>
		/// This is the object the behavior is currently attached to. This will be null if no object is attached.
		/// </summary>
		private FrameworkElement m_attachedObject;

		/// <summary>
		/// The VisualBrush that is used directly on the background/fill of the attached object.
		/// It's visual is the background Visual.
		/// </summary>
		private readonly VisualBrush m_attachedObjectVisualBrush = new VisualBrush();

		/// <summary>
		/// This is the visual that is used to apply the effect on and is filled with the background VisualBrush.
		/// </summary>
		private readonly Rectangle m_backgroundVisual = new Rectangle();

		/// <summary>
		/// This is the VisualBrush of the background.
		/// </summary>
		private readonly VisualBrush m_backgroundVisualBrush = new VisualBrush();
		#endregion
	}
}
