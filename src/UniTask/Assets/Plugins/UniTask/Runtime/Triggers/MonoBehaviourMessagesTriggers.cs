#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading;
using UnityEngine;
#if UNITASK_UGUI_SUPPORT
using UnityEngine.EventSystems;
#endif

namespace Cysharp.Threading.Tasks.Triggers
{
#region BeforeTransformParentChanged

    public interface IAsyncOnBeforeTransformParentChangedHandler
    {
        UniTask OnBeforeTransformParentChangedAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnBeforeTransformParentChangedHandler
    {
        UniTask IAsyncOnBeforeTransformParentChangedHandler.OnBeforeTransformParentChangedAsync()
        {
            core.Reset();
            return new UniTask((IUniTaskSource)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncBeforeTransformParentChangedTrigger GetAsyncBeforeTransformParentChangedTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncBeforeTransformParentChangedTrigger>(gameObject);
        }
        
        public static AsyncBeforeTransformParentChangedTrigger GetAsyncBeforeTransformParentChangedTrigger(this Component component)
        {
            return component.gameObject.GetAsyncBeforeTransformParentChangedTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncBeforeTransformParentChangedTrigger : AsyncTriggerBase<AsyncUnit>
    {
        void OnBeforeTransformParentChanged()
        {
            RaiseEvent(AsyncUnit.Default);
        }

        public IAsyncOnBeforeTransformParentChangedHandler GetOnBeforeTransformParentChangedAsyncHandler()
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, false);
        }

        public IAsyncOnBeforeTransformParentChangedHandler GetOnBeforeTransformParentChangedAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
        }

        public UniTask OnBeforeTransformParentChangedAsync()
        {
            return ((IAsyncOnBeforeTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnBeforeTransformParentChangedAsync();
        }

        public UniTask OnBeforeTransformParentChangedAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnBeforeTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnBeforeTransformParentChangedAsync();
        }
    }
#endregion

#region Disable

    public interface IAsyncOnDisableHandler
    {
        UniTask OnDisableAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnDisableHandler
    {
        UniTask IAsyncOnDisableHandler.OnDisableAsync()
        {
            core.Reset();
            return new UniTask((IUniTaskSource)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncDisableTrigger GetAsyncDisableTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncDisableTrigger>(gameObject);
        }
        
        public static AsyncDisableTrigger GetAsyncDisableTrigger(this Component component)
        {
            return component.gameObject.GetAsyncDisableTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncDisableTrigger : AsyncTriggerBase<AsyncUnit>
    {
        void OnDisable()
        {
            RaiseEvent(AsyncUnit.Default);
        }

        public IAsyncOnDisableHandler GetOnDisableAsyncHandler()
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, false);
        }

        public IAsyncOnDisableHandler GetOnDisableAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
        }

        public UniTask OnDisableAsync()
        {
            return ((IAsyncOnDisableHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnDisableAsync();
        }

        public UniTask OnDisableAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnDisableHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnDisableAsync();
        }
    }
#endregion

#region Enable

    public interface IAsyncOnEnableHandler
    {
        UniTask OnEnableAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnEnableHandler
    {
        UniTask IAsyncOnEnableHandler.OnEnableAsync()
        {
            core.Reset();
            return new UniTask((IUniTaskSource)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncEnableTrigger GetAsyncEnableTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncEnableTrigger>(gameObject);
        }
        
        public static AsyncEnableTrigger GetAsyncEnableTrigger(this Component component)
        {
            return component.gameObject.GetAsyncEnableTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncEnableTrigger : AsyncTriggerBase<AsyncUnit>
    {
        void OnEnable()
        {
            RaiseEvent(AsyncUnit.Default);
        }

        public IAsyncOnEnableHandler GetOnEnableAsyncHandler()
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, false);
        }

        public IAsyncOnEnableHandler GetOnEnableAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
        }

        public UniTask OnEnableAsync()
        {
            return ((IAsyncOnEnableHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnEnableAsync();
        }

        public UniTask OnEnableAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnEnableHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnEnableAsync();
        }
    }
#endregion

#region RectTransformDimensionsChange

    public interface IAsyncOnRectTransformDimensionsChangeHandler
    {
        UniTask OnRectTransformDimensionsChangeAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnRectTransformDimensionsChangeHandler
    {
        UniTask IAsyncOnRectTransformDimensionsChangeHandler.OnRectTransformDimensionsChangeAsync()
        {
            core.Reset();
            return new UniTask((IUniTaskSource)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncRectTransformDimensionsChangeTrigger GetAsyncRectTransformDimensionsChangeTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncRectTransformDimensionsChangeTrigger>(gameObject);
        }
        
        public static AsyncRectTransformDimensionsChangeTrigger GetAsyncRectTransformDimensionsChangeTrigger(this Component component)
        {
            return component.gameObject.GetAsyncRectTransformDimensionsChangeTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncRectTransformDimensionsChangeTrigger : AsyncTriggerBase<AsyncUnit>
    {
        void OnRectTransformDimensionsChange()
        {
            RaiseEvent(AsyncUnit.Default);
        }

        public IAsyncOnRectTransformDimensionsChangeHandler GetOnRectTransformDimensionsChangeAsyncHandler()
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, false);
        }

        public IAsyncOnRectTransformDimensionsChangeHandler GetOnRectTransformDimensionsChangeAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
        }

        public UniTask OnRectTransformDimensionsChangeAsync()
        {
            return ((IAsyncOnRectTransformDimensionsChangeHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnRectTransformDimensionsChangeAsync();
        }

        public UniTask OnRectTransformDimensionsChangeAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnRectTransformDimensionsChangeHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnRectTransformDimensionsChangeAsync();
        }
    }
#endregion

#region TransformChildrenChanged

    public interface IAsyncOnTransformChildrenChangedHandler
    {
        UniTask OnTransformChildrenChangedAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnTransformChildrenChangedHandler
    {
        UniTask IAsyncOnTransformChildrenChangedHandler.OnTransformChildrenChangedAsync()
        {
            core.Reset();
            return new UniTask((IUniTaskSource)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncTransformChildrenChangedTrigger GetAsyncTransformChildrenChangedTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncTransformChildrenChangedTrigger>(gameObject);
        }
        
        public static AsyncTransformChildrenChangedTrigger GetAsyncTransformChildrenChangedTrigger(this Component component)
        {
            return component.gameObject.GetAsyncTransformChildrenChangedTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncTransformChildrenChangedTrigger : AsyncTriggerBase<AsyncUnit>
    {
        void OnTransformChildrenChanged()
        {
            RaiseEvent(AsyncUnit.Default);
        }

        public IAsyncOnTransformChildrenChangedHandler GetOnTransformChildrenChangedAsyncHandler()
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, false);
        }

        public IAsyncOnTransformChildrenChangedHandler GetOnTransformChildrenChangedAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
        }

        public UniTask OnTransformChildrenChangedAsync()
        {
            return ((IAsyncOnTransformChildrenChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnTransformChildrenChangedAsync();
        }

        public UniTask OnTransformChildrenChangedAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnTransformChildrenChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnTransformChildrenChangedAsync();
        }
    }
#endregion

#region TransformParentChanged

    public interface IAsyncOnTransformParentChangedHandler
    {
        UniTask OnTransformParentChangedAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnTransformParentChangedHandler
    {
        UniTask IAsyncOnTransformParentChangedHandler.OnTransformParentChangedAsync()
        {
            core.Reset();
            return new UniTask((IUniTaskSource)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncTransformParentChangedTrigger GetAsyncTransformParentChangedTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncTransformParentChangedTrigger>(gameObject);
        }
        
        public static AsyncTransformParentChangedTrigger GetAsyncTransformParentChangedTrigger(this Component component)
        {
            return component.gameObject.GetAsyncTransformParentChangedTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncTransformParentChangedTrigger : AsyncTriggerBase<AsyncUnit>
    {
        void OnTransformParentChanged()
        {
            RaiseEvent(AsyncUnit.Default);
        }

        public IAsyncOnTransformParentChangedHandler GetOnTransformParentChangedAsyncHandler()
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, false);
        }

        public IAsyncOnTransformParentChangedHandler GetOnTransformParentChangedAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
        }

        public UniTask OnTransformParentChangedAsync()
        {
            return ((IAsyncOnTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).OnTransformParentChangedAsync();
        }

        public UniTask OnTransformParentChangedAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnTransformParentChangedHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).OnTransformParentChangedAsync();
        }
    }
#endregion

#region Update

    public interface IAsyncUpdateHandler
    {
        UniTask UpdateAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncUpdateHandler
    {
        UniTask IAsyncUpdateHandler.UpdateAsync()
        {
            core.Reset();
            return new UniTask((IUniTaskSource)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncUpdateTrigger GetAsyncUpdateTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncUpdateTrigger>(gameObject);
        }
        
        public static AsyncUpdateTrigger GetAsyncUpdateTrigger(this Component component)
        {
            return component.gameObject.GetAsyncUpdateTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncUpdateTrigger : AsyncTriggerBase<AsyncUnit>
    {
        void Update()
        {
            RaiseEvent(AsyncUnit.Default);
        }

        public IAsyncUpdateHandler GetUpdateAsyncHandler()
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, false);
        }

        public IAsyncUpdateHandler GetUpdateAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, false);
        }

        public UniTask UpdateAsync()
        {
            return ((IAsyncUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, true)).UpdateAsync();
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncUpdateHandler)new AsyncTriggerHandler<AsyncUnit>(this, cancellationToken, true)).UpdateAsync();
        }
    }
#endregion

#region BeginDrag
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnBeginDragHandler
    {
        UniTask<PointerEventData> OnBeginDragAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnBeginDragHandler
    {
        UniTask<PointerEventData> IAsyncOnBeginDragHandler.OnBeginDragAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncBeginDragTrigger GetAsyncBeginDragTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncBeginDragTrigger>(gameObject);
        }
        
        public static AsyncBeginDragTrigger GetAsyncBeginDragTrigger(this Component component)
        {
            return component.gameObject.GetAsyncBeginDragTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncBeginDragTrigger : AsyncTriggerBase<PointerEventData>, IBeginDragHandler
    {
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnBeginDragHandler GetOnBeginDragAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnBeginDragHandler GetOnBeginDragAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnBeginDragAsync()
        {
            return ((IAsyncOnBeginDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnBeginDragAsync();
        }

        public UniTask<PointerEventData> OnBeginDragAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnBeginDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnBeginDragAsync();
        }
    }
#endif
#endregion

#region Drag
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnDragHandler
    {
        UniTask<PointerEventData> OnDragAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnDragHandler
    {
        UniTask<PointerEventData> IAsyncOnDragHandler.OnDragAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncDragTrigger GetAsyncDragTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncDragTrigger>(gameObject);
        }
        
        public static AsyncDragTrigger GetAsyncDragTrigger(this Component component)
        {
            return component.gameObject.GetAsyncDragTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncDragTrigger : AsyncTriggerBase<PointerEventData>, IDragHandler
    {
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnDragHandler GetOnDragAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnDragHandler GetOnDragAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnDragAsync()
        {
            return ((IAsyncOnDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnDragAsync();
        }

        public UniTask<PointerEventData> OnDragAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnDragAsync();
        }
    }
#endif
#endregion

#region Drop
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnDropHandler
    {
        UniTask<PointerEventData> OnDropAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnDropHandler
    {
        UniTask<PointerEventData> IAsyncOnDropHandler.OnDropAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncDropTrigger GetAsyncDropTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncDropTrigger>(gameObject);
        }
        
        public static AsyncDropTrigger GetAsyncDropTrigger(this Component component)
        {
            return component.gameObject.GetAsyncDropTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncDropTrigger : AsyncTriggerBase<PointerEventData>, IDropHandler
    {
        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnDropHandler GetOnDropAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnDropHandler GetOnDropAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnDropAsync()
        {
            return ((IAsyncOnDropHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnDropAsync();
        }

        public UniTask<PointerEventData> OnDropAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnDropHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnDropAsync();
        }
    }
#endif
#endregion

#region EndDrag
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnEndDragHandler
    {
        UniTask<PointerEventData> OnEndDragAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnEndDragHandler
    {
        UniTask<PointerEventData> IAsyncOnEndDragHandler.OnEndDragAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncEndDragTrigger GetAsyncEndDragTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncEndDragTrigger>(gameObject);
        }
        
        public static AsyncEndDragTrigger GetAsyncEndDragTrigger(this Component component)
        {
            return component.gameObject.GetAsyncEndDragTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncEndDragTrigger : AsyncTriggerBase<PointerEventData>, IEndDragHandler
    {
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnEndDragHandler GetOnEndDragAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnEndDragHandler GetOnEndDragAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnEndDragAsync()
        {
            return ((IAsyncOnEndDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnEndDragAsync();
        }

        public UniTask<PointerEventData> OnEndDragAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnEndDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnEndDragAsync();
        }
    }
#endif
#endregion

#region InitializePotentialDrag
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnInitializePotentialDragHandler
    {
        UniTask<PointerEventData> OnInitializePotentialDragAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnInitializePotentialDragHandler
    {
        UniTask<PointerEventData> IAsyncOnInitializePotentialDragHandler.OnInitializePotentialDragAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncInitializePotentialDragTrigger GetAsyncInitializePotentialDragTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncInitializePotentialDragTrigger>(gameObject);
        }
        
        public static AsyncInitializePotentialDragTrigger GetAsyncInitializePotentialDragTrigger(this Component component)
        {
            return component.gameObject.GetAsyncInitializePotentialDragTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncInitializePotentialDragTrigger : AsyncTriggerBase<PointerEventData>, IInitializePotentialDragHandler
    {
        void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnInitializePotentialDragHandler GetOnInitializePotentialDragAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnInitializePotentialDragHandler GetOnInitializePotentialDragAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnInitializePotentialDragAsync()
        {
            return ((IAsyncOnInitializePotentialDragHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnInitializePotentialDragAsync();
        }

        public UniTask<PointerEventData> OnInitializePotentialDragAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnInitializePotentialDragHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnInitializePotentialDragAsync();
        }
    }
#endif
#endregion

#region PointerClick
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnPointerClickHandler
    {
        UniTask<PointerEventData> OnPointerClickAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnPointerClickHandler
    {
        UniTask<PointerEventData> IAsyncOnPointerClickHandler.OnPointerClickAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncPointerClickTrigger GetAsyncPointerClickTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncPointerClickTrigger>(gameObject);
        }
        
        public static AsyncPointerClickTrigger GetAsyncPointerClickTrigger(this Component component)
        {
            return component.gameObject.GetAsyncPointerClickTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncPointerClickTrigger : AsyncTriggerBase<PointerEventData>, IPointerClickHandler
    {
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnPointerClickHandler GetOnPointerClickAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnPointerClickHandler GetOnPointerClickAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnPointerClickAsync()
        {
            return ((IAsyncOnPointerClickHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerClickAsync();
        }

        public UniTask<PointerEventData> OnPointerClickAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnPointerClickHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerClickAsync();
        }
    }
#endif
#endregion

#region PointerDown
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnPointerDownHandler
    {
        UniTask<PointerEventData> OnPointerDownAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnPointerDownHandler
    {
        UniTask<PointerEventData> IAsyncOnPointerDownHandler.OnPointerDownAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncPointerDownTrigger GetAsyncPointerDownTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncPointerDownTrigger>(gameObject);
        }
        
        public static AsyncPointerDownTrigger GetAsyncPointerDownTrigger(this Component component)
        {
            return component.gameObject.GetAsyncPointerDownTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncPointerDownTrigger : AsyncTriggerBase<PointerEventData>, IPointerDownHandler
    {
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnPointerDownHandler GetOnPointerDownAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnPointerDownHandler GetOnPointerDownAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnPointerDownAsync()
        {
            return ((IAsyncOnPointerDownHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerDownAsync();
        }

        public UniTask<PointerEventData> OnPointerDownAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnPointerDownHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerDownAsync();
        }
    }
#endif
#endregion

#region PointerEnter
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnPointerEnterHandler
    {
        UniTask<PointerEventData> OnPointerEnterAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnPointerEnterHandler
    {
        UniTask<PointerEventData> IAsyncOnPointerEnterHandler.OnPointerEnterAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncPointerEnterTrigger GetAsyncPointerEnterTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncPointerEnterTrigger>(gameObject);
        }
        
        public static AsyncPointerEnterTrigger GetAsyncPointerEnterTrigger(this Component component)
        {
            return component.gameObject.GetAsyncPointerEnterTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncPointerEnterTrigger : AsyncTriggerBase<PointerEventData>, IPointerEnterHandler
    {
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnPointerEnterHandler GetOnPointerEnterAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnPointerEnterHandler GetOnPointerEnterAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnPointerEnterAsync()
        {
            return ((IAsyncOnPointerEnterHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerEnterAsync();
        }

        public UniTask<PointerEventData> OnPointerEnterAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnPointerEnterHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerEnterAsync();
        }
    }
#endif
#endregion

#region PointerExit
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnPointerExitHandler
    {
        UniTask<PointerEventData> OnPointerExitAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnPointerExitHandler
    {
        UniTask<PointerEventData> IAsyncOnPointerExitHandler.OnPointerExitAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncPointerExitTrigger GetAsyncPointerExitTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncPointerExitTrigger>(gameObject);
        }
        
        public static AsyncPointerExitTrigger GetAsyncPointerExitTrigger(this Component component)
        {
            return component.gameObject.GetAsyncPointerExitTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncPointerExitTrigger : AsyncTriggerBase<PointerEventData>, IPointerExitHandler
    {
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnPointerExitHandler GetOnPointerExitAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnPointerExitHandler GetOnPointerExitAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnPointerExitAsync()
        {
            return ((IAsyncOnPointerExitHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerExitAsync();
        }

        public UniTask<PointerEventData> OnPointerExitAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnPointerExitHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerExitAsync();
        }
    }
#endif
#endregion

#region PointerUp
#if UNITASK_UGUI_SUPPORT

    public interface IAsyncOnPointerUpHandler
    {
        UniTask<PointerEventData> OnPointerUpAsync();
    }

    public partial class AsyncTriggerHandler<T> : IAsyncOnPointerUpHandler
    {
        UniTask<PointerEventData> IAsyncOnPointerUpHandler.OnPointerUpAsync()
        {
            core.Reset();
            return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)(object)this, core.Version);
        }
    }

    public static partial class AsyncTriggerExtensions
    {
        public static AsyncPointerUpTrigger GetAsyncPointerUpTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncPointerUpTrigger>(gameObject);
        }
        
        public static AsyncPointerUpTrigger GetAsyncPointerUpTrigger(this Component component)
        {
            return component.gameObject.GetAsyncPointerUpTrigger();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AsyncPointerUpTrigger : AsyncTriggerBase<PointerEventData>, IPointerUpHandler
    {
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            RaiseEvent((eventData));
        }

        public IAsyncOnPointerUpHandler GetOnPointerUpAsyncHandler()
        {
            return new AsyncTriggerHandler<PointerEventData>(this, false);
        }

        public IAsyncOnPointerUpHandler GetOnPointerUpAsyncHandler(CancellationToken cancellationToken)
        {
            return new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, false);
        }

        public UniTask<PointerEventData> OnPointerUpAsync()
        {
            return ((IAsyncOnPointerUpHandler)new AsyncTriggerHandler<PointerEventData>(this, true)).OnPointerUpAsync();
        }

        public UniTask<PointerEventData> OnPointerUpAsync(CancellationToken cancellationToken)
        {
            return ((IAsyncOnPointerUpHandler)new AsyncTriggerHandler<PointerEventData>(this, cancellationToken, true)).OnPointerUpAsync();
        }
    }
#endif
#endregion

}