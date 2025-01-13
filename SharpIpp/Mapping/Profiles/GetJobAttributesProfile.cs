using System;
using System.Collections.Generic;
using System.Linq;

using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Extensions;
using SharpIpp.Protocol.Models;

namespace SharpIpp.Mapping.Profiles
{
    // ReSharper disable once UnusedMember.Global
    internal class GetJobAttributesProfile : IProfile
    {
        public void CreateMaps(IMapperConstructor mapper)
        {
            mapper.CreateMap<GetJobAttributesRequest, IppRequestMessage>((src, map) =>
            {
                var dst = new IppRequestMessage { IppOperation = IppOperation.GetJobAttributes };
                map.Map<IIppJobRequest, IppRequestMessage>(src, dst);
                if (src.OperationAttributes != null)
                    dst.OperationAttributes.AddRange(src.OperationAttributes.GetIppAttributes(map));
                dst.JobAttributes.Populate(src.AdditionalJobAttributes);
                return dst;
            });

            mapper.CreateMap<IIppRequestMessage, GetJobAttributesRequest>( ( src, map ) =>
            {
                var dst = new GetJobAttributesRequest();
                map.Map<IIppRequestMessage, IIppJobRequest>( src, dst );
                dst.OperationAttributes = GetJobAttributesOperationAttributes.Create<GetJobAttributesOperationAttributes>(src.OperationAttributes.ToIppDictionary(), map);
                var additionalJobAttributes = src.JobAttributes.Where( x => !JobAttribute.GetAttributes( src.Version ).Contains( x.Name ) ).ToList();
                if (additionalJobAttributes.Any())
                    dst.AdditionalJobAttributes = additionalJobAttributes;
                return dst;
            } );

            //https://tools.ietf.org/html/rfc2911#section-4.4
            mapper.CreateMap<IppResponseMessage, GetJobAttributesResponse>((src, map) =>
            {
                var dst = new GetJobAttributesResponse { JobAttributes = map.Map<JobDescriptionAttributes>(src.AllAttributes()) };
                map.Map<IppResponseMessage, IIppResponseMessage>(src, dst);
                return dst;
            });

            mapper.CreateMap<GetJobAttributesResponse, IppResponseMessage>( ( src, map ) =>
            {
                var dst = new IppResponseMessage();
                map.Map<IIppResponseMessage, IppResponseMessage>( src, dst );
                var section = new IppSection { Tag = SectionTag.JobAttributesTag };
                section.Attributes.AddRange( map.Map<IDictionary<string, IppAttribute[]>>( src.JobAttributes ).Values.SelectMany( x => x ) );
                dst.Sections.Add(section );
                return dst;
            } );

            mapper.CreateMap<IDictionary<string, IppAttribute[]>, JobDescriptionAttributes>((src, map) => new JobDescriptionAttributes
            {
                Copies = map.MapFromDic<int?>(src, JobAttribute.Copies),
                DateTimeAtCompleted = map.MapFromDic<DateTimeOffset?>(src, JobAttribute.DateTimeAtCompleted),
                DateTimeAtCreation = map.MapFromDic<DateTimeOffset?>(src, JobAttribute.DateTimeAtCreation),
                DateTimeAtProcessing = map.MapFromDic<DateTimeOffset?>(src, JobAttribute.DateTimeAtProcessing),
                Finishings = map.MapFromDic<Finishings?>(src, JobAttribute.Finishings),
                JobId = map.MapFromDic<int?>(src, JobAttribute.JobId),
                JobUri = map.MapFromDic<string?>(src, JobAttribute.JobUri),
                JobImpressionsCompleted = map.MapFromDic<int?>(src, JobAttribute.JobImpressionsCompleted),
                JobMediaSheetsCompleted = map.MapFromDic<int?>(src, JobAttribute.JobMediaSheetsCompleted),
                JobOriginatingUserName = map.MapFromDic<string?>(src, JobAttribute.JobOriginatingUserName),
                JobOriginatingUserNameLanguage =
                    map.MapFromDicLanguage(src, JobAttribute.JobOriginatingUserNameLanguage),
                JobPrinterUpTime = map.MapFromDic<int?>(src, JobAttribute.JobPrinterUpTime),
                JobPrinterUri = map.MapFromDic<string?>(src, JobAttribute.JobPrinterUri),
                JobSheets = map.MapFromDic<JobSheets?>(src, JobAttribute.JobSheets),
                JobState = map.MapFromDic<JobState?>(src, JobAttribute.JobState),
                JobStateMessage = map.MapFromDic<string?>(src, JobAttribute.JobStateMessage),
                JobStateReasons = map.MapFromDicSetNull<JobStateReason[]?>(src, JobAttribute.JobStateReasons),
                Media = map.MapFromDic<string?>(src, JobAttribute.Media),
                MultipleDocumentHandling =
                    map.MapFromDic<MultipleDocumentHandling?>(src, JobAttribute.MultipleDocumentHandling),
                NumberUp = map.MapFromDic<int?>(src, JobAttribute.NumberUp),
                OrientationRequested = map.MapFromDic<Orientation?>(src, JobAttribute.OrientationRequested),
                PrinterResolution = map.MapFromDic<Resolution?>(src, JobAttribute.PrinterResolution),
                PrintQuality = map.MapFromDic<PrintQuality?>(src, JobAttribute.PrintQuality),
                Sides = map.MapFromDic<Sides?>(src, JobAttribute.Sides),
                TimeAtCompleted = map.MapFromDic<int?>(src, JobAttribute.TimeAtCompleted),
                TimeAtCreation = map.MapFromDic<int?>(src, JobAttribute.TimeAtCreation),
                TimeAtProcessing = map.MapFromDic<int?>(src, JobAttribute.TimeAtProcessing),
            });

            mapper.CreateMap<JobDescriptionAttributes, IDictionary<string, IppAttribute[]>>( ( src, map ) =>
            {
                var dic = new Dictionary<string, IppAttribute[]>();
                if ( src.Copies != null )
                    dic.Add( JobAttribute.Copies, new IppAttribute[] { new IppAttribute( Tag.Integer, JobAttribute.Copies, src.Copies.Value ) } );
                if (src.DateTimeAtCompleted != null)
                {
                    if (src.DateTimeAtCompleted.Value > DateTimeOffset.MinValue)
                        dic.Add(JobAttribute.DateTimeAtCompleted, new IppAttribute[] { new IppAttribute(Tag.DateTime, JobAttribute.DateTimeAtCompleted, src.DateTimeAtCompleted.Value) });
                    else
                        dic.Add(JobAttribute.DateTimeAtCompleted, new IppAttribute[] { new IppAttribute(Tag.NoValue, JobAttribute.DateTimeAtCompleted, NoValue.Instance) });
                }
                if (src.DateTimeAtCreation != null)
                {
                    if (src.DateTimeAtCreation.Value > DateTimeOffset.MinValue)
                        dic.Add(JobAttribute.DateTimeAtCreation, new IppAttribute[] { new IppAttribute(Tag.DateTime, JobAttribute.DateTimeAtCreation, src.DateTimeAtCreation.Value) });
                    else
                        dic.Add(JobAttribute.DateTimeAtCreation, new IppAttribute[] { new IppAttribute(Tag.NoValue, JobAttribute.DateTimeAtCreation, NoValue.Instance) });
                }
                if (src.DateTimeAtProcessing != null)
                {
                    if (src.DateTimeAtProcessing.Value > DateTimeOffset.MinValue)
                        dic.Add(JobAttribute.DateTimeAtProcessing, new IppAttribute[] { new IppAttribute(Tag.DateTime, JobAttribute.DateTimeAtProcessing, src.DateTimeAtProcessing.Value) });
                    else
                        dic.Add(JobAttribute.DateTimeAtProcessing, new IppAttribute[] { new IppAttribute(Tag.NoValue, JobAttribute.DateTimeAtProcessing, NoValue.Instance) });
                }
                if ( src.Finishings != null )
                    dic.Add( JobAttribute.Finishings, new IppAttribute[] { new IppAttribute( Tag.Enum, JobAttribute.Finishings, (int)src.Finishings.Value ) } );
                if ( src.JobId != null )
                    dic.Add( JobAttribute.JobId, new IppAttribute[] { new IppAttribute( Tag.Integer, JobAttribute.JobId, src.JobId.Value ) } );
                if( src.JobUri != null )
                    dic.Add( JobAttribute.JobUri, new IppAttribute[] { new IppAttribute( Tag.Uri, JobAttribute.JobUri, src.JobUri ) } );
                if ( src.JobImpressionsCompleted != null )
                    dic.Add( JobAttribute.JobImpressionsCompleted, new IppAttribute[] { new IppAttribute( Tag.Integer, JobAttribute.JobImpressionsCompleted, src.JobImpressionsCompleted.Value ) } );
                if ( src.JobMediaSheetsCompleted != null )
                    dic.Add( JobAttribute.JobMediaSheetsCompleted, new IppAttribute[] { new IppAttribute( Tag.Integer, JobAttribute.JobMediaSheetsCompleted, src.JobMediaSheetsCompleted.Value ) } );
                if ( src.JobOriginatingUserName != null )
                    dic.Add( JobAttribute.JobOriginatingUserName, new IppAttribute[] { new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.JobOriginatingUserName, src.JobOriginatingUserName ) } );
                if ( src.JobOriginatingUserNameLanguage != null )
                    dic.Add( JobAttribute.JobOriginatingUserNameLanguage, new IppAttribute[] { new IppAttribute( Tag.NaturalLanguage, JobAttribute.JobOriginatingUserNameLanguage, src.JobOriginatingUserNameLanguage ) } );
                if ( src.JobPrinterUpTime != null )
                    dic.Add( JobAttribute.JobPrinterUpTime, new IppAttribute[] { new IppAttribute( Tag.Integer, JobAttribute.JobPrinterUpTime, src.JobPrinterUpTime.Value ) } );
                if ( src.JobPrinterUri != null )
                    dic.Add( JobAttribute.JobPrinterUri, new IppAttribute[] { new IppAttribute( Tag.Uri, JobAttribute.JobPrinterUri, src.JobPrinterUri ) } );
                if ( src.JobSheets != null )
                    dic.Add( JobAttribute.JobSheets, new IppAttribute[] { new IppAttribute( Tag.Keyword, JobAttribute.JobSheets, map.Map<string>( src.JobSheets ) ) } );
                if ( src.JobState != null )
                    dic.Add( JobAttribute.JobState, new IppAttribute[] { new IppAttribute( Tag.Enum, JobAttribute.JobState, (int)src.JobState.Value ) } );
                if ( src.JobStateMessage != null )
                    dic.Add( JobAttribute.JobStateMessage, new IppAttribute[] { new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.JobStateMessage, src.JobStateMessage ) } );
                if (src.JobStateReasons?.Any() ?? false)
                    dic.Add( JobAttribute.JobStateReasons, src.JobStateReasons.Select( x => new IppAttribute( Tag.Keyword, JobAttribute.JobStateReasons, map.Map<string>( x ) ) ).ToArray() );
                if ( src.Media != null )
                    dic.Add( JobAttribute.Media, new IppAttribute[] { new IppAttribute( Tag.Keyword, JobAttribute.Media, src.Media ) } );
                if ( src.MultipleDocumentHandling != null )
                    dic.Add( JobAttribute.MultipleDocumentHandling, new IppAttribute[] { new IppAttribute( Tag.Keyword, JobAttribute.MultipleDocumentHandling, map.Map<string>( src.MultipleDocumentHandling ) ) } );
                if ( src.NumberUp != null )
                    dic.Add( JobAttribute.NumberUp, new IppAttribute[] { new IppAttribute( Tag.Integer, JobAttribute.NumberUp, src.NumberUp.Value ) } );
                if ( src.OrientationRequested != null )
                    dic.Add( JobAttribute.OrientationRequested, new IppAttribute[] { new IppAttribute( Tag.Enum, JobAttribute.OrientationRequested, (int)src.OrientationRequested.Value ) } );
                if ( src.PrinterResolution != null )
                    dic.Add( JobAttribute.PrinterResolution, new IppAttribute[] { new IppAttribute( Tag.Resolution, JobAttribute.PrinterResolution, src.PrinterResolution.Value ) } );
                if ( src.PrintQuality != null )
                    dic.Add( JobAttribute.PrintQuality, new IppAttribute[] { new IppAttribute( Tag.Enum, JobAttribute.PrintQuality, (int)src.PrintQuality.Value ) } );
                if ( src.Sides != null )
                    dic.Add( JobAttribute.Sides, new IppAttribute[] { new IppAttribute( Tag.Keyword, JobAttribute.Sides, map.Map<string>( src.Sides ) ) } );
                if (src.TimeAtCompleted != null)
                {
                    if (src.TimeAtCompleted.Value >= 0)
                        dic.Add(JobAttribute.TimeAtCompleted, new IppAttribute[] { new IppAttribute(Tag.Integer, JobAttribute.TimeAtCompleted, src.TimeAtCompleted.Value) });
                    else
                        dic.Add(JobAttribute.TimeAtCompleted, new IppAttribute[] { new IppAttribute(Tag.NoValue, JobAttribute.TimeAtCompleted, NoValue.Instance) });
                }
                if (src.TimeAtCreation != null)
                {
                    if (src.TimeAtCreation.Value >= 0)
                        dic.Add(JobAttribute.TimeAtCreation, new IppAttribute[] { new IppAttribute(Tag.Integer, JobAttribute.TimeAtCreation, src.TimeAtCreation.Value) });
                    else
                        dic.Add(JobAttribute.TimeAtCreation, new IppAttribute[] { new IppAttribute(Tag.NoValue, JobAttribute.TimeAtCreation, NoValue.Instance) });
                }
                if (src.TimeAtProcessing != null)
                {
                    if (src.TimeAtProcessing.Value >= 0)
                        dic.Add(JobAttribute.TimeAtProcessing, new IppAttribute[] { new IppAttribute(Tag.Integer, JobAttribute.TimeAtProcessing, src.TimeAtProcessing.Value) });
                    else
                        dic.Add(JobAttribute.TimeAtProcessing, new IppAttribute[] { new IppAttribute(Tag.NoValue, JobAttribute.TimeAtProcessing, NoValue.Instance) });
                }
                return dic;
            } );
        }
    }
}
