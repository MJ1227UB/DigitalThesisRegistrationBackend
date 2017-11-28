﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTRDAL.Context;
using DTRDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DTRDAL.Repositories.Implementations
{
    class StudentRepository: IStudentRepository
    {
        private readonly DTRContext _context;

        public StudentRepository(DTRContext context)
        {
            _context = context;
        }

        public Student Create(Student ent)
        {
            return _context.Students.Add(ent).Entity;
        }

        public Student Get(int id)
        {
            return _context.Students.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Students;
        }

        public IEnumerable<Student> GetByIds(List<int> ids)
        {
            return _context.Students.Where(s => ids.Contains(s.Id));
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
