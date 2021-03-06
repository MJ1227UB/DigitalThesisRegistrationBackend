﻿using System.Collections.Generic;
using System.Linq;
using DTRBLL.BusinessObjects;
using DTRDAL.Entities;

namespace DTRBLL.Converters.Implementations
{
    public class GroupConverter : IConverter<Group, GroupBO>
    {

        private readonly StudentConverter _studentConverter = new StudentConverter();
        public Group Convert(GroupBO bo)
        {
            if (bo == null) return null;
            return new Group
            {
                Id = bo.Id,
                UserId = bo.UserId,
                ContactEmail = bo.ContactEmail,
                Students = bo.Students?.Select(_studentConverter.Convert).ToList()
            };
        }

        public GroupBO Convert(Group entity)
        {
            if (entity == null) return null;
            return new GroupBO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ContactEmail = entity.ContactEmail,
                Students = entity.Students?.Select(_studentConverter.Convert).ToList()
            };
        }
    }
}