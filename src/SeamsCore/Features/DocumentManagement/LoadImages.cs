using AutoMapper;
using MediatR;
using SeamsCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace SeamsCore.Features.DocumentManagement
{
    public class LoadImages
    {
        public class Query : IAsyncRequest<Result>
        {
            public List<string> AllowedExtensions { get; set; }
            public string ImagesDirectory { get; set; }
            public string SubDirectory { get; set; }
        }

        public class Result
        {
            public string SubDirectory { get; set; }
            public List<string> Files { get; set; }
            public List<string> Directories { get; set; }
        }
        
        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            public async Task<Result> Handle(Query message)
            {
                DirectoryInfo dir = new DirectoryInfo(message.ImagesDirectory);

                var images = 
                    dir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly)
                    .OrderBy(d => d.CreationTime)
                    .Select(d => d.FullName)
                    .Distinct()
                    .Where(s => message.AllowedExtensions.Any(ext => ext == Path.GetExtension(s)))
                    .ToList();

                //do not allow sub directories past the first level (avoiding use of the cms as a full featured file storage solution)
                var subDirs = (message.SubDirectory == null || message.SubDirectory.Length <= 0) 
                    ? dir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                        .OrderBy(d => d.CreationTime)
                        .Select(d => d.Name)
                        .Distinct()
                        .ToList()
                    : new List<string>();

                var result = new Result
                {
                    SubDirectory = message.SubDirectory,
                    Files = images,
                    Directories = subDirs
                };

                return result;
            }
        }
    }
}
