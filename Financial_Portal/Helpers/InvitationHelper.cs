using Financial_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial_Portal.Helpers
{
    public class InvitationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void MarkAsInvalid(int invitationId)
        {
            var invitation = db.Invitations.Find(invitationId);
            invitation.IsValid = false;
            db.SaveChanges();
        }
    }
}