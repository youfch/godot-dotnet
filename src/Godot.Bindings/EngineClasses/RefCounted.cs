using System.Threading;

namespace Godot;

partial class RefCounted
{
    // Tracks whether this instance is referenced by the C# side.
    // In the case where the instance is created from C# this should be true from the start.
    // In the case where the instance is created from GDScript, this will be set to true when
    // the marshaller retrieves it for the first time, since that's when C# acquires a reference to it.
    private bool _ownsRef;

    /// <summary>
    /// Mark this instance as already referenced by the C# side.
    /// </summary>
    /// <remarks>
    /// IMPORTANT: This should only be called when the reference count has already been incremented
    /// to account for the C# reference, it indicates that this instance has a reference on the C# side
    /// and must not increment the reference count again.
    /// </remarks>
    internal void MarkAsReferenceOwned()
    {
        _ownsRef = true;
    }

    /// <summary>
    /// Increment the reference count for this instance to account for the C# reference.
    /// If this instance has already incremented the reference count for the C# reference,
    /// it won't be incremented again.
    /// </summary>
    internal void InitRefOnlyOnce()
    {
        if (Interlocked.Exchange(ref _ownsRef, true))
        {
            return;
        }

        InitRef();
    }
}
