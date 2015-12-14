using System;
using System.Collections.Generic;
using System.Linq;
using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.DAL.Repositories.Abstract;
using Example.Services.Models;
using Example.Services.Services.Abstract;

namespace Example.Services.Services
{
    public interface ISectionService : IIntService<Section>
    {
        ExampleSection GetRootSections();
        ExampleSection Find(int id);
        void Save(ExampleSection model);
        void Remove(int id);
        int? Add(ExampleSection model);
    }

    public class SectionService : IntService<Section>, ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IDbContextProvider _provider;

        public SectionService(ISectionRepository repository, IDbContextProvider provider) : base(repository, provider)
        {
            _sectionRepository = repository;
            _provider = provider;
        }

        public ExampleSection GetRootSections()
        {
            ExampleSection forum = new ExampleSection();

            var collection = _sectionRepository.AsQueryable().Where(s => s.ParentId == null).ToList();
            forum.ChildSections = collection.Select(s => 
                new ExampleSection
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    CreatedDate = s.CreatedDate,
                    ModifiedDate = s.ModifiedDate
                })
                .ToList();

            return forum;
        }

        public ExampleSection Find(int id)
        {
            var entity = _sectionRepository.FindById(id);
            if (entity == null)
            {
                return null;
            }

            var section = new ExampleSection(entity, true);

            if (entity.Topics != null && entity.Topics.Any())
            {
                section.Topics = entity.Topics.Select(t => new ExampleTopic(t, section)).ToList();
            }
            else
            {
                section.Topics = new List<ExampleTopic>();
            }

            return section;
        }

        public void Save(ExampleSection model)
        {
            var section = _sectionRepository.FindById(model.Id);
            if (section == null)
                throw new NullReferenceException();

            section.Title = model.Title;
            section.Description = model.Description;
            _sectionRepository.Update(section);
            _provider.SaveChanges();
        }

        public void Remove(int id)
        {
            var section = _sectionRepository.FindById(id);
            if (section == null)
                throw new NullReferenceException();

            _sectionRepository.Delete(id);
            _provider.SaveChanges();
        }

        public int? Add(ExampleSection model)
        {
            try
            {
                var entity = new Section
                {
                    Title = model.Title,
                    Description = model.Description,
                    ParentId = model.ParentSectionId
                };
                var newEntity = _sectionRepository.Insert(entity);
                _provider.SaveChanges();
                return newEntity.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
