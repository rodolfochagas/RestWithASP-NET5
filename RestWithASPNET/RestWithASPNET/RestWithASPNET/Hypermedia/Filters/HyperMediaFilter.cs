using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNET.Hypermedia.Filters
{
    public class HyperMediaFilter : ResultFilterAttribute
    {
        private readonly HyperMediaFilterOptions _hiperMediaFilterOptions;

        public HyperMediaFilter(HyperMediaFilterOptions hiperMediaFilterOptions)
        {
            _hiperMediaFilterOptions = hiperMediaFilterOptions;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            TryEnrichResult(context);
            base.OnResultExecuting(context);
        }

        private void TryEnrichResult(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult objectResult)
            {
                //object result sendo instanciado no if que verifica a resposta do contexto???
                var enricher = _hiperMediaFilterOptions
                    .ContentResponseEnricherList
                    .FirstOrDefault(x => x.CanEnrich(context));
                if (enricher != null) Task.FromResult(enricher.Enrich(context));
            }
        }
    }
}
