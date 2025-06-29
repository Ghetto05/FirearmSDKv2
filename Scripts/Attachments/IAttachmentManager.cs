using System.Collections.Generic;

namespace GhettosFirearmSDKv2.Attachments
{
    public interface IAttachmentManager
    {
        List<AttachmentPoint> AttachmentPoints { get; set; }
    
        List<Attachment> CurrentAttachments { get; set; }
    
        public delegate void Collision(UnityEngine.Collision collision);
        public event Collision OnCollision;

        public delegate void AttachmentAdded(Attachment attachment, AttachmentPoint attachmentPoint);
        public event AttachmentAdded OnAttachmentAdded;

        public delegate void AttachmentRemoved(Attachment attachment, AttachmentPoint attachmentPoint);
        public event AttachmentRemoved OnAttachmentRemoved;
    }
}