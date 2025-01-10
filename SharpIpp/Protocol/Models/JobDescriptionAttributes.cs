using System;

namespace SharpIpp.Protocol.Models
{
    public class JobDescriptionAttributes
    {
        /// <summary>
        ///     This REQUIRED attribute contains the ID of the job.  The Printer, on
        ///     receipt of a new job, generates an ID which identifies the new Job on
        ///     that Printer.  The Printer returns the value of the "job-id"
        ///     attribute as part of the response to a create request.  The 0 value
        ///     is not included to allow for compatibility with SNMP index values
        ///     which also cannot be 0.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.2
        /// </summary>
        /// <example>63</example>
        /// <code>job-id</code>
        public int? JobId { get; set; }

        /// <summary>
        /// https://tools.ietf.org/html/rfc2911#section-4.3.1
        /// </summary>
        public string? JobUri { get; set; }

        /// <summary>
        ///     This REQUIRED attribute identifies the Printer object that created
        ///     this Job object.  When a Printer object creates a Job object, it
        ///     populates this attribute with the Printer object URI that was used in
        ///     the create request.  This attribute permits a client to identify the
        ///     Printer object that created this Job object when only the Job
        ///     object's URI is available to the client.  The client queries the
        ///     creating Printer object to determine which languages, charsets,
        ///     operations, are supported for this Job.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.3
        /// </summary>
        /// <example>ipp://10.30.254.250:631/ipp/print</example>
        /// <code>job-printer-uri</code>
        public string? JobPrinterUri { get; set; }

        /// <summary>
        /// https://tools.ietf.org/html/rfc2911#section-4.3.4
        /// </summary>
        public string? JobMoreInfo { get; set; }

        /// <summary>
        ///     This REQUIRED attribute contains the name of the end user that
        ///     submitted the print job.  The Printer object sets this attribute to
        ///     the most authenticated printable name that it can obtain from the
        ///     authentication service over which the IPP operation was received.
        ///     Only if such is not available, does the Printer object use the value
        ///     supplied by the client in the "requesting-user-name" operation
        ///     attribute of the create operation (see Sections 4.4.2, 4.4.3, and 8).
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.6
        /// </summary>
        /// <example>anonymous (en)</example>
        /// <code>job-originating-user-name</code>
        public string? JobOriginatingUserName { get; set; }

        /// <summary>
        ///     if <see cref="JobOriginatingUserName" /> had a language, this property stores it
        /// </summary>
        public string? JobOriginatingUserNameLanguage { get; set; }

        /// <summary>
        ///     This attribute determines which job start/end sheet(s), if any, MUST
        ///     be printed with a job.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.3
        /// </summary>
        /// <example>none</example>
        /// <code>job-sheets</code>
        public JobSheets? JobSheets { get; set; }

        /// <summary>
        ///     This attribute specifies the number of copies to be printed.
        ///     On many devices the supported number of collated copies will be
        ///     limited by the number of physical output bins on the device, and may
        ///     be different from the number of uncollated copies which can be
        ///     supported.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.5
        /// </summary>
        /// <example>1</example>
        /// <code>copies</code>
        public int? Copies { get; set; }

        /// <summary>
        ///     This attribute is relevant only if a job consists of two or more
        ///     documents. This attribute MUST be supported with at least one value
        ///     if the Printer supports multiple documents per job (see sections
        ///     3.2.4 and 3.3.1).  The attribute controls finishing operations and
        ///     the placement of one or more print-stream pages into impressions and
        ///     onto media sheets.  When the value of the "copies" attribute exceeds
        ///     1, it also controls the order in which the copies that result from
        ///     processing the documents are produced. For the purposes of this
        ///     explanations, if "a" represents an instance of document data, then
        ///     the result of processing the data in document "a" is a sequence of
        ///     media sheets represented by "a(*)".
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.4
        /// </summary>
        /// <example>separate-documents-uncollated-copies</example>
        /// <code>multiple-document-handling</code>
        public MultipleDocumentHandling? MultipleDocumentHandling { get; set; }

        /// <summary>
        ///     This attribute specifies the print quality that the Printer uses for
        ///     the Job.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.13
        /// </summary>
        /// <example>4</example>
        /// <code>print-quality</code>
        public PrintQuality? PrintQuality { get; set; }

        /// <summary>
        ///     This attribute identifies the resolution that Printer uses for the
        ///     Job.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.12
        /// </summary>
        /// <example>600x600 (dpi)</example>
        /// <code>printer-resolution</code>
        public Resolution? PrinterResolution { get; set; }

        /// <summary>
        ///     This attribute specifies how print-stream pages are to be imposed
        ///     upon the sides of an instance of a selected medium, i.e., an
        ///     impression.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.8
        /// </summary>
        /// <example>one-sided</example>
        /// <code>sides</code>
        public Sides? Sides { get; set; }

        /// <summary>
        ///     This attribute identifies the medium that the Printer uses for all
        ///     impressions of the Job.
        ///     The values for "media" include medium-names, medium-sizes, input-
        ///     trays and electronic forms so that one attribute specifies the media.
        ///     If a Printer object supports a medium name as a value of this
        ///     attribute, such a medium name implicitly selects an input-tray that
        ///     contains the specified medium.  If a Printer object supports a medium
        ///     size as a value of this attribute, such a medium size implicitly
        ///     selects a medium name that in turn implicitly selects an input-tray
        ///     that contains the medium with the specified size.  If a Printer
        ///     object supports an input-tray as the value of this attribute, such an
        ///     input-tray implicitly selects the medium that is in that input-tray
        ///     at the time the job prints.  This case includes manual-feed input-
        ///     trays.  If a Printer object supports an electronic form as the value
        ///     of this attribute, such an electronic form implicitly selects a
        ///     medium-name that in turn implicitly selects an input-tray that
        ///     contains the medium specified by the electronic form.  The electronic
        ///     form also implicitly selects an image that the Printer MUST merge
        ///     with the document data as its prints each page.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.11
        /// </summary>
        /// <example>iso_a4_210x297mm</example>
        /// <code>media</code>
        public string? Media { get; set; }

        /// <summary>
        ///     This attribute specifies the number of print-stream pages to impose
        ///     upon a single side of an instance of a selected medium.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.9
        /// </summary>
        /// <example>1</example>
        /// <code>number-up</code>
        public int? NumberUp { get; set; }

        /// <summary>
        ///     This attribute indicates the desired orientation for printed print-
        ///     stream pages; it does not describe the orientation of the client-
        ///     supplied print-stream pages.
        ///     For some document formats (such as 'application/postscript'), the
        ///     desired orientation of the print-stream pages is specified within the
        ///     document data.  This information is generated by a device driver
        ///     prior to the submission of the print job.  Other document formats
        ///     (such as 'text/plain') do not include the notion of desired
        ///     orientation within the document data.  In the latter case it is
        ///     possible for the Printer object to bind the desired orientation to
        ///     the document data after it has been submitted.  It is expected that a
        ///     Printer object would only support "orientations-requested" for some
        ///     document formats (e.g., 'text/plain' or 'text/html') but not others
        ///     (e.g., 'application/postscript').  This is no different than any
        ///     other Job Template attribute since section 4.2, item 1, points out
        ///     that a Printer object may support or not support any Job Template
        ///     attribute based on the document format supplied by the client.
        ///     However, a special mention is made here since it is very likely that
        ///     a Printer object will support "orientation-requested" for only a
        ///     subset of the supported document formats.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.10
        /// </summary>
        /// <example>3</example>
        /// <code>orientation-requested</code>
        public Orientation? OrientationRequested { get; set; }

        /// <summary>
        ///     This attribute identifies the finishing operations that the Printer
        ///     uses for each copy of each printed document in the Job. For Jobs with
        ///     multiple documents, the "multiple-document-handling" attribute
        ///     determines what constitutes a "copy" for purposes of finishing.
        ///     https://tools.ietf.org/html/rfc2911#section-4.2.6
        /// </summary>
        /// <example>3</example>
        /// <code>finishings</code>
        public Finishings? Finishings { get; set; }

        

        

        /// <summary>
        ///     This job attribute specifies the number of impressions completed for
        ///     the job so far.  For printing devices, the impressions completed
        ///     includes interpreting, marking, and stacking the output.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.18.2
        /// </summary>
        /// <example>0</example>
        /// <code>job-impressions-completed</code>
        public int? JobImpressionsCompleted { get; set; }

        

        /// <summary>
        ///     This job attribute specifies the media-sheets completed marking and
        ///     stacking for the entire job so far whether those sheets have been
        ///     processed on one side or on both.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.18.3
        /// </summary>
        /// <example>0</example>
        /// <code>job-media-sheets-completed</code>
        public int? JobMediaSheetsCompleted { get; set; }

        /// <summary>
        ///     This REQUIRED attribute identifies the current state of the job.
        ///     Even though the IPP protocol defines seven values for job states
        ///     (plus the out-of-band 'unknown' value - see Section 4.1),
        ///     implementations only need to support those states which are
        ///     appropriate for the particular implementation.  In other words, a
        ///     Printer supports only those job states implemented by the output
        ///     device and available to the Printer object implementation.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.7
        /// </summary>
        /// <example>9</example>
        /// <code>job-state</code>
        public JobState? JobState { get; set; }

        /// <summary>
        ///     The Printer object OPTIONALLY returns the Job object's OPTIONAL
        ///     "job-state-message" attribute.  If the Printer object supports
        ///     this attribute then it MUST be returned in the response.  If
        ///     this attribute is not returned in the response, the client can
        ///     assume that the "job-state-message" attribute is not supported
        ///     and will not be returned in a subsequent Job object query.
        /// </summary>
        /// <example>The job completed successfully</example>
        /// <code>job-state-message</code>
        public string? JobStateMessage { get; set; }

        /// <summary>
        ///     The Printer object MUST return the Job object's REQUIRED "job-
        ///     state-reasons" attribute.
        /// </summary>
        /// <example>job-completed-successfully</example>
        /// <code>job-state-reasons</code>
        public JobStateReason[]? JobStateReasons { get; set; }

        /// <summary>
        ///     This attribute indicates the date and time at which the Job object
        ///     was created.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.14.5
        /// </summary>
        /// <example>22.04.2021 20:13:21 +03:00</example>
        /// <code>date-time-at-creation</code>
        public DateTimeOffset? DateTimeAtCreation { get; set; }

        /// <summary>
        ///     This attribute indicates the date and time at which the Job object
        ///     first began processing after the create operation or the most recent
        ///     Restart-Job operation.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.14.6
        /// </summary>
        /// <example>22.04.2021 20:13:22 +03:00</example>
        /// <code>date-time-at-processing</code>
        public DateTimeOffset? DateTimeAtProcessing { get; set; }

        /// <summary>
        ///     This attribute indicates the date and time at which the Job object
        ///     completed (or was canceled or aborted).
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.14.7
        /// </summary>
        /// <example>22.04.2021 20:13:22 +03:00</example>
        /// <code>date-time-at-completed</code>
        public DateTimeOffset? DateTimeAtCompleted { get; set; }

        /// <summary>
        ///     This REQUIRED attribute indicates the time at which the Job object
        ///     was created.
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.14.1
        /// </summary>
        /// <example>197753</example>
        /// <code>time-at-creation</code>
        public int? TimeAtCreation { get; set; }

        /// <summary>
        ///     This REQUIRED attribute indicates the time at which the Job object
        ///     first began processing after the create operation or the most recent
        ///     Restart-Job operation.  The out-of-band 'no-value' value is returned
        ///     if the job has not yet been in the 'processing' state
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.14.2
        /// </summary>
        /// <example>197754</example>
        /// <code>time-at-processing</code>
        public int? TimeAtProcessing { get; set; }

        /// <summary>
        ///     This REQUIRED attribute indicates the time at which the Job object
        ///     completed (or was canceled or aborted).  The out-of-band 'no-value'
        ///     value is returned if the job has not yet completed, been canceled, or
        ///     aborted
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.14.3
        /// </summary>
        /// <example>197754</example>
        /// <code>time-at-completed</code>
        public int? TimeAtCompleted { get; set; }

        /// <summary>
        ///     This REQUIRED Job Description attribute indicates the amount of time
        ///     (in seconds) that the Printer implementation has been up and running.
        ///     This attribute is an alias for the "printer-up-time" Printer
        ///     Description attribute (see Section 4.4.29).
        ///     https://tools.ietf.org/html/rfc2911#section-4.3.14.4
        /// </summary>
        /// <example>197775</example>
        /// <code>job-printer-up-time</code>
        public int? JobPrinterUpTime { get; set; }
    }
}
