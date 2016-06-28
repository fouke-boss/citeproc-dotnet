using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// The name of the term.
    /// </summary>
    public enum TermName
    {
        // ----------------------------------------------
        // -- Locators                                 --
        // ----------------------------------------------

        /// <summary>
        /// book
        /// </summary>
        [XmlEnum("book")]
        Book,
        /// <summary>
        /// chapter
        /// </summary>
        [XmlEnum("chapter")]
        Chapter,
        /// <summary>
        /// column
        /// </summary>
        [XmlEnum("column")]
        Column,
        /// <summary>
        /// figure
        /// </summary>
        [XmlEnum("figure")]
        Figure,
        /// <summary>
        /// folio
        /// </summary>
        [XmlEnum("folio")]
        Folio,
        /// <summary>
        /// issue
        /// </summary>
        [XmlEnum("issue")]
        Issue,
        /// <summary>
        /// line
        /// </summary>
        [XmlEnum("line")]
        Line,
        /// <summary>
        /// note
        /// </summary>
        [XmlEnum("note")]
        Note,
        /// <summary>
        /// opus
        /// </summary>
        [XmlEnum("opus")]
        Opus,
        /// <summary>
        /// page
        /// </summary>
        [XmlEnum("page")]
        Page,
        /// <summary>
        /// paragraph
        /// </summary>
        [XmlEnum("paragraph")]
        Paragraph,
        /// <summary>
        /// part
        /// </summary>
        [XmlEnum("part")]
        Part,
        /// <summary>
        /// section
        /// </summary>
        [XmlEnum("section")]
        Section,
        /// <summary>
        /// sub verbo
        /// </summary>
        [XmlEnum("sub verbo")]
        SubVerbo,
        /// <summary>
        /// verse
        /// </summary>
        [XmlEnum("verse")]
        Verse,
        /// <summary>
        /// volume
        /// </summary>
        [XmlEnum("volume")]
        Volume,


        // ----------------------------------------------
        // -- Months                                   --
        // ----------------------------------------------

        /// <summary>
        /// month-01
        /// </summary>
        [XmlEnum("month-01")]
        Month01,
        /// <summary>
        /// month-02
        /// </summary>
        [XmlEnum("month-02")]
        Month02,
        /// <summary>
        /// month-03
        /// </summary>
        [XmlEnum("month-03")]
        Month03,
        /// <summary>
        /// month-04
        /// </summary>
        [XmlEnum("month-04")]
        Month04,
        /// <summary>
        /// month-05
        /// </summary>
        [XmlEnum("month-05")]
        Month05,
        /// <summary>
        /// month-06
        /// </summary>
        [XmlEnum("month-06")]
        Month06,
        /// <summary>
        /// month-07
        /// </summary>
        [XmlEnum("month-07")]
        Month07,
        /// <summary>
        /// month-08
        /// </summary>
        [XmlEnum("month-08")]
        Month08,
        /// <summary>
        /// month-09
        /// </summary>
        [XmlEnum("month-09")]
        Month09,
        /// <summary>
        /// month-10
        /// </summary>
        [XmlEnum("month-10")]
        Month10,
        /// <summary>
        /// month-11
        /// </summary>
        [XmlEnum("month-11")]
        Month11,
        /// <summary>
        /// month-12
        /// </summary>
        [XmlEnum("month-12")]
        Month12,


        // ----------------------------------------------
        // -- Ordinals                                 --
        // ----------------------------------------------

        /// <summary>
        /// ordinal
        /// </summary>
        [XmlEnum("ordinal")]
        Ordinal,
        /// <summary>
        /// long-ordinal-01
        /// </summary>
        [XmlEnum("long-ordinal-01")]
        LongOrdinal01,
        /// <summary>
        /// long-ordinal-02
        /// </summary>
        [XmlEnum("long-ordinal-02")]
        LongOrdinal02,
        /// <summary>
        /// long-ordinal-03
        /// </summary>
        [XmlEnum("long-ordinal-03")]
        LongOrdinal03,
        /// <summary>
        /// long-ordinal-04
        /// </summary>
        [XmlEnum("long-ordinal-04")]
        LongOrdinal04,
        /// <summary>
        /// long-ordinal-05
        /// </summary>
        [XmlEnum("long-ordinal-05")]
        LongOrdinal05,
        /// <summary>
        /// long-ordinal-06
        /// </summary>
        [XmlEnum("long-ordinal-06")]
        LongOrdinal06,
        /// <summary>
        /// long-ordinal-07
        /// </summary>
        [XmlEnum("long-ordinal-07")]
        LongOrdinal07,
        /// <summary>
        /// long-ordinal-08
        /// </summary>
        [XmlEnum("long-ordinal-08")]
        LongOrdinal08,
        /// <summary>
        /// long-ordinal-09
        /// </summary>
        [XmlEnum("long-ordinal-09")]
        LongOrdinal09,
        /// <summary>
        /// long-ordinal-10
        /// </summary>
        [XmlEnum("long-ordinal-10")]
        LongOrdinal10,


        // ----------------------------------------------
        // -- Ordinals 00-99                           --
        // ----------------------------------------------

        /// <summary>
        /// ordinal-00
        /// </summary>
        [XmlEnum("ordinal-00")]
        Ordinal00,
        /// <summary>
        /// ordinal-01
        /// </summary>
        [XmlEnum("ordinal-01")]
        Ordinal01,
        /// <summary>
        /// ordinal-02
        /// </summary>
        [XmlEnum("ordinal-02")]
        Ordinal02,
        /// <summary>
        /// ordinal-03
        /// </summary>
        [XmlEnum("ordinal-03")]
        Ordinal03,
        /// <summary>
        /// ordinal-04
        /// </summary>
        [XmlEnum("ordinal-04")]
        Ordinal04,
        /// <summary>
        /// ordinal-05
        /// </summary>
        [XmlEnum("ordinal-05")]
        Ordinal05,
        /// <summary>
        /// ordinal-06
        /// </summary>
        [XmlEnum("ordinal-06")]
        Ordinal06,
        /// <summary>
        /// ordinal-07
        /// </summary>
        [XmlEnum("ordinal-07")]
        Ordinal07,
        /// <summary>
        /// ordinal-08
        /// </summary>
        [XmlEnum("ordinal-08")]
        Ordinal08,
        /// <summary>
        /// ordinal-09
        /// </summary>
        [XmlEnum("ordinal-09")]
        Ordinal09,
        /// <summary>
        /// ordinal-10
        /// </summary>
        [XmlEnum("ordinal-10")]
        Ordinal10,
        /// <summary>
        /// ordinal-11
        /// </summary>
        [XmlEnum("ordinal-11")]
        Ordinal11,
        /// <summary>
        /// ordinal-12
        /// </summary>
        [XmlEnum("ordinal-12")]
        Ordinal12,
        /// <summary>
        /// ordinal-13
        /// </summary>
        [XmlEnum("ordinal-13")]
        Ordinal13,
        /// <summary>
        /// ordinal-14
        /// </summary>
        [XmlEnum("ordinal-14")]
        Ordinal14,
        /// <summary>
        /// ordinal-15
        /// </summary>
        [XmlEnum("ordinal-15")]
        Ordinal15,
        /// <summary>
        /// ordinal-16
        /// </summary>
        [XmlEnum("ordinal-16")]
        Ordinal16,
        /// <summary>
        /// ordinal-17
        /// </summary>
        [XmlEnum("ordinal-17")]
        Ordinal17,
        /// <summary>
        /// ordinal-18
        /// </summary>
        [XmlEnum("ordinal-18")]
        Ordinal18,
        /// <summary>
        /// ordinal-19
        /// </summary>
        [XmlEnum("ordinal-19")]
        Ordinal19,
        /// <summary>
        /// ordinal-20
        /// </summary>
        [XmlEnum("ordinal-20")]
        Ordinal20,
        /// <summary>
        /// ordinal-21
        /// </summary>
        [XmlEnum("ordinal-21")]
        Ordinal21,
        /// <summary>
        /// ordinal-22
        /// </summary>
        [XmlEnum("ordinal-22")]
        Ordinal22,
        /// <summary>
        /// ordinal-23
        /// </summary>
        [XmlEnum("ordinal-23")]
        Ordinal23,
        /// <summary>
        /// ordinal-24
        /// </summary>
        [XmlEnum("ordinal-24")]
        Ordinal24,
        /// <summary>
        /// ordinal-25
        /// </summary>
        [XmlEnum("ordinal-25")]
        Ordinal25,
        /// <summary>
        /// ordinal-26
        /// </summary>
        [XmlEnum("ordinal-26")]
        Ordinal26,
        /// <summary>
        /// ordinal-27
        /// </summary>
        [XmlEnum("ordinal-27")]
        Ordinal27,
        /// <summary>
        /// ordinal-28
        /// </summary>
        [XmlEnum("ordinal-28")]
        Ordinal28,
        /// <summary>
        /// ordinal-29
        /// </summary>
        [XmlEnum("ordinal-29")]
        Ordinal29,
        /// <summary>
        /// ordinal-30
        /// </summary>
        [XmlEnum("ordinal-30")]
        Ordinal30,
        /// <summary>
        /// ordinal-31
        /// </summary>
        [XmlEnum("ordinal-31")]
        Ordinal31,
        /// <summary>
        /// ordinal-32
        /// </summary>
        [XmlEnum("ordinal-32")]
        Ordinal32,
        /// <summary>
        /// ordinal-33
        /// </summary>
        [XmlEnum("ordinal-33")]
        Ordinal33,
        /// <summary>
        /// ordinal-34
        /// </summary>
        [XmlEnum("ordinal-34")]
        Ordinal34,
        /// <summary>
        /// ordinal-35
        /// </summary>
        [XmlEnum("ordinal-35")]
        Ordinal35,
        /// <summary>
        /// ordinal-36
        /// </summary>
        [XmlEnum("ordinal-36")]
        Ordinal36,
        /// <summary>
        /// ordinal-37
        /// </summary>
        [XmlEnum("ordinal-37")]
        Ordinal37,
        /// <summary>
        /// ordinal-38
        /// </summary>
        [XmlEnum("ordinal-38")]
        Ordinal38,
        /// <summary>
        /// ordinal-39
        /// </summary>
        [XmlEnum("ordinal-39")]
        Ordinal39,
        /// <summary>
        /// ordinal-40
        /// </summary>
        [XmlEnum("ordinal-40")]
        Ordinal40,
        /// <summary>
        /// ordinal-41
        /// </summary>
        [XmlEnum("ordinal-41")]
        Ordinal41,
        /// <summary>
        /// ordinal-42
        /// </summary>
        [XmlEnum("ordinal-42")]
        Ordinal42,
        /// <summary>
        /// ordinal-43
        /// </summary>
        [XmlEnum("ordinal-43")]
        Ordinal43,
        /// <summary>
        /// ordinal-44
        /// </summary>
        [XmlEnum("ordinal-44")]
        Ordinal44,
        /// <summary>
        /// ordinal-45
        /// </summary>
        [XmlEnum("ordinal-45")]
        Ordinal45,
        /// <summary>
        /// ordinal-46
        /// </summary>
        [XmlEnum("ordinal-46")]
        Ordinal46,
        /// <summary>
        /// ordinal-47
        /// </summary>
        [XmlEnum("ordinal-47")]
        Ordinal47,
        /// <summary>
        /// ordinal-48
        /// </summary>
        [XmlEnum("ordinal-48")]
        Ordinal48,
        /// <summary>
        /// ordinal-49
        /// </summary>
        [XmlEnum("ordinal-49")]
        Ordinal49,
        /// <summary>
        /// ordinal-50
        /// </summary>
        [XmlEnum("ordinal-50")]
        Ordinal50,
        /// <summary>
        /// ordinal-51
        /// </summary>
        [XmlEnum("ordinal-51")]
        Ordinal51,
        /// <summary>
        /// ordinal-52
        /// </summary>
        [XmlEnum("ordinal-52")]
        Ordinal52,
        /// <summary>
        /// ordinal-53
        /// </summary>
        [XmlEnum("ordinal-53")]
        Ordinal53,
        /// <summary>
        /// ordinal-54
        /// </summary>
        [XmlEnum("ordinal-54")]
        Ordinal54,
        /// <summary>
        /// ordinal-55
        /// </summary>
        [XmlEnum("ordinal-55")]
        Ordinal55,
        /// <summary>
        /// ordinal-56
        /// </summary>
        [XmlEnum("ordinal-56")]
        Ordinal56,
        /// <summary>
        /// ordinal-57
        /// </summary>
        [XmlEnum("ordinal-57")]
        Ordinal57,
        /// <summary>
        /// ordinal-58
        /// </summary>
        [XmlEnum("ordinal-58")]
        Ordinal58,
        /// <summary>
        /// ordinal-59
        /// </summary>
        [XmlEnum("ordinal-59")]
        Ordinal59,
        /// <summary>
        /// ordinal-60
        /// </summary>
        [XmlEnum("ordinal-60")]
        Ordinal60,
        /// <summary>
        /// ordinal-61
        /// </summary>
        [XmlEnum("ordinal-61")]
        Ordinal61,
        /// <summary>
        /// ordinal-62
        /// </summary>
        [XmlEnum("ordinal-62")]
        Ordinal62,
        /// <summary>
        /// ordinal-63
        /// </summary>
        [XmlEnum("ordinal-63")]
        Ordinal63,
        /// <summary>
        /// ordinal-64
        /// </summary>
        [XmlEnum("ordinal-64")]
        Ordinal64,
        /// <summary>
        /// ordinal-65
        /// </summary>
        [XmlEnum("ordinal-65")]
        Ordinal65,
        /// <summary>
        /// ordinal-66
        /// </summary>
        [XmlEnum("ordinal-66")]
        Ordinal66,
        /// <summary>
        /// ordinal-67
        /// </summary>
        [XmlEnum("ordinal-67")]
        Ordinal67,
        /// <summary>
        /// ordinal-68
        /// </summary>
        [XmlEnum("ordinal-68")]
        Ordinal68,
        /// <summary>
        /// ordinal-69
        /// </summary>
        [XmlEnum("ordinal-69")]
        Ordinal69,
        /// <summary>
        /// ordinal-70
        /// </summary>
        [XmlEnum("ordinal-70")]
        Ordinal70,
        /// <summary>
        /// ordinal-71
        /// </summary>
        [XmlEnum("ordinal-71")]
        Ordinal71,
        /// <summary>
        /// ordinal-72
        /// </summary>
        [XmlEnum("ordinal-72")]
        Ordinal72,
        /// <summary>
        /// ordinal-73
        /// </summary>
        [XmlEnum("ordinal-73")]
        Ordinal73,
        /// <summary>
        /// ordinal-74
        /// </summary>
        [XmlEnum("ordinal-74")]
        Ordinal74,
        /// <summary>
        /// ordinal-75
        /// </summary>
        [XmlEnum("ordinal-75")]
        Ordinal75,
        /// <summary>
        /// ordinal-76
        /// </summary>
        [XmlEnum("ordinal-76")]
        Ordinal76,
        /// <summary>
        /// ordinal-77
        /// </summary>
        [XmlEnum("ordinal-77")]
        Ordinal77,
        /// <summary>
        /// ordinal-78
        /// </summary>
        [XmlEnum("ordinal-78")]
        Ordinal78,
        /// <summary>
        /// ordinal-79
        /// </summary>
        [XmlEnum("ordinal-79")]
        Ordinal79,
        /// <summary>
        /// ordinal-80
        /// </summary>
        [XmlEnum("ordinal-80")]
        Ordinal80,
        /// <summary>
        /// ordinal-81
        /// </summary>
        [XmlEnum("ordinal-81")]
        Ordinal81,
        /// <summary>
        /// ordinal-82
        /// </summary>
        [XmlEnum("ordinal-82")]
        Ordinal82,
        /// <summary>
        /// ordinal-83
        /// </summary>
        [XmlEnum("ordinal-83")]
        Ordinal83,
        /// <summary>
        /// ordinal-84
        /// </summary>
        [XmlEnum("ordinal-84")]
        Ordinal84,
        /// <summary>
        /// ordinal-85
        /// </summary>
        [XmlEnum("ordinal-85")]
        Ordinal85,
        /// <summary>
        /// ordinal-86
        /// </summary>
        [XmlEnum("ordinal-86")]
        Ordinal86,
        /// <summary>
        /// ordinal-87
        /// </summary>
        [XmlEnum("ordinal-87")]
        Ordinal87,
        /// <summary>
        /// ordinal-88
        /// </summary>
        [XmlEnum("ordinal-88")]
        Ordinal88,
        /// <summary>
        /// ordinal-89
        /// </summary>
        [XmlEnum("ordinal-89")]
        Ordinal89,
        /// <summary>
        /// ordinal-90
        /// </summary>
        [XmlEnum("ordinal-90")]
        Ordinal90,
        /// <summary>
        /// ordinal-91
        /// </summary>
        [XmlEnum("ordinal-91")]
        Ordinal91,
        /// <summary>
        /// ordinal-92
        /// </summary>
        [XmlEnum("ordinal-92")]
        Ordinal92,
        /// <summary>
        /// ordinal-93
        /// </summary>
        [XmlEnum("ordinal-93")]
        Ordinal93,
        /// <summary>
        /// ordinal-94
        /// </summary>
        [XmlEnum("ordinal-94")]
        Ordinal94,
        /// <summary>
        /// ordinal-95
        /// </summary>
        [XmlEnum("ordinal-95")]
        Ordinal95,
        /// <summary>
        /// ordinal-96
        /// </summary>
        [XmlEnum("ordinal-96")]
        Ordinal96,
        /// <summary>
        /// ordinal-97
        /// </summary>
        [XmlEnum("ordinal-97")]
        Ordinal97,
        /// <summary>
        /// ordinal-98
        /// </summary>
        [XmlEnum("ordinal-98")]
        Ordinal98,
        /// <summary>
        /// ordinal-99
        /// </summary>
        [XmlEnum("ordinal-99")]
        Ordinal99,


        // ----------------------------------------------
        // -- Quotation marks                          --
        // ----------------------------------------------

        /// <summary>
        /// open-quote
        /// </summary>
        [XmlEnum("open-quote")]
        OpenQuote,
        /// <summary>
        /// close-quote
        /// </summary>
        [XmlEnum("close-quote")]
        CloseQuote,
        /// <summary>
        /// open-inner-quote
        /// </summary>
        [XmlEnum("open-inner-quote")]
        OpenInnerQuote,
        /// <summary>
        /// close-inner-quote
        /// </summary>
        [XmlEnum("close-inner-quote")]
        CloseInnerQuote,


        // ----------------------------------------------
        // -- Roles                                    --
        // ----------------------------------------------

        /// <summary>
        /// author
        /// </summary>
        [XmlEnum("author")]
        Author,
        /// <summary>
        /// collection-editor
        /// </summary>
        [XmlEnum("collection-editor")]
        CollectionEditor,
        /// <summary>
        /// composer
        /// </summary>
        [XmlEnum("composer")]
        Composer,
        /// <summary>
        /// container-author
        /// </summary>
        [XmlEnum("container-author")]
        ContainerAuthor,
        /// <summary>
        /// director
        /// </summary>
        [XmlEnum("director")]
        Director,
        /// <summary>
        /// editor
        /// </summary>
        [XmlEnum("editor")]
        Editor,
        /// <summary>
        /// editorial-director
        /// </summary>
        [XmlEnum("editorial-director")]
        EditorialDirector,
        /// <summary>
        /// editortranslator
        /// </summary>
        [XmlEnum("editortranslator")]
        EditorTranslator,
        /// <summary>
        /// illustrator
        /// </summary>
        [XmlEnum("illustrator")]
        Illustrator,
        /// <summary>
        /// interviewer
        /// </summary>
        [XmlEnum("interviewer")]
        Interviewer,
        /// <summary>
        /// original-author
        /// </summary>
        [XmlEnum("original-author")]
        OriginalAuthor,
        /// <summary>
        /// recipient
        /// </summary>
        [XmlEnum("recipient")]
        Recipient,
        /// <summary>
        /// reviewed-author
        /// </summary>
        [XmlEnum("reviewed-author")]
        ReviewedAuthor,
        /// <summary>
        /// translator
        /// </summary>
        [XmlEnum("translator")]
        Translator,


        // ----------------------------------------------
        // -- Seasons                                  --
        // ----------------------------------------------

        /// <summary>
        /// season-01
        /// </summary>
        [XmlEnum("season-01")]
        Season01,
        /// <summary>
        /// season-02
        /// </summary>
        [XmlEnum("season-02")]
        Season02,
        /// <summary>
        /// season-03
        /// </summary>
        [XmlEnum("season-03")]
        Season03,
        /// <summary>
        /// season-04
        /// </summary>
        [XmlEnum("season-04")]
        Season04,


        // ----------------------------------------------
        // -- Miscellaneous                            --
        // ----------------------------------------------

        /// <summary>
        /// accessed
        /// </summary>
        [XmlEnum("accessed")]
        Accessed,
        /// <summary>
        /// ad
        /// </summary>
        [XmlEnum("ad")]
        Ad,
        /// <summary>
        /// and
        /// </summary>
        [XmlEnum("and")]
        And,
        /// <summary>
        /// and others
        /// </summary>
        [XmlEnum("and others")]
        AndOthers,
        /// <summary>
        /// anonymous
        /// </summary>
        [XmlEnum("anonymous")]
        Anonymous,
        /// <summary>
        /// at
        /// </summary>
        [XmlEnum("at")]
        At,
        /// <summary>
        /// available at
        /// </summary>
        [XmlEnum("available at")]
        AvailableAt,
        /// <summary>
        /// bc
        /// </summary>
        [XmlEnum("bc")]
        Bc,
        /// <summary>
        /// by
        /// </summary>
        [XmlEnum("by")]
        By,
        /// <summary>
        /// circa
        /// </summary>
        [XmlEnum("circa")]
        Circa,
        /// <summary>
        /// cited
        /// </summary>
        [XmlEnum("cited")]
        Cited,
        /// <summary>
        /// chapter-number
        /// </summary>
        [XmlEnum("chapter-number")]
        ChapterNumber,
        /// <summary>
        /// collection-number
        /// </summary>
        [XmlEnum("collection-number")]
        CollectionNumber,
        /// <summary>
        /// edition
        /// </summary>
        [XmlEnum("edition")]
        Edition,
        /// <summary>
        /// et-al
        /// </summary>
        [XmlEnum("et-al")]
        EtAl,
        /// <summary>
        /// forthcoming
        /// </summary>
        [XmlEnum("forthcoming")]
        Forthcoming,
        /// <summary>
        /// from
        /// </summary>
        [XmlEnum("from")]
        From,
        /// <summary>
        /// ibid
        /// </summary>
        [XmlEnum("ibid")]
        Ibid,
        /// <summary>
        /// in
        /// </summary>
        [XmlEnum("in")]
        In,
        /// <summary>
        /// in press
        /// </summary>
        [XmlEnum("in press")]
        InPress,
        /// <summary>
        /// internet
        /// </summary>
        [XmlEnum("internet")]
        Internet,
        /// <summary>
        /// interview
        /// </summary>
        [XmlEnum("interview")]
        Interview,
        /// <summary>
        /// letter
        /// </summary>
        [XmlEnum("letter")]
        Letter,
        /// <summary>
        /// locator
        /// </summary>
        [XmlEnum("locator")]
        Locator,
        /// <summary>
        /// no date
        /// </summary>
        [XmlEnum("no date")]
        NoDate,
        /// <summary>
        /// number-of-pages
        /// </summary>
        [XmlEnum("number-of-pages")]
        NumberOfPages,
        /// <summary>
        /// number-of-volumes
        /// </summary>
        [XmlEnum("number-of-volumes")]
        NumberOfVolumes,
        /// <summary>
        /// online
        /// </summary>
        [XmlEnum("online")]
        Online,
        /// <summary>
        /// page-range-delimiter
        /// </summary>
        [XmlEnum("page-range-delimiter")]
        PageRangeDelimiter,
        /// <summary>
        /// presented at
        /// </summary>
        [XmlEnum("presented at")]
        PresentedAt,
        /// <summary>
        /// reference
        /// </summary>
        [XmlEnum("reference")]
        Reference,
        /// <summary>
        /// retrieved
        /// </summary>
        [XmlEnum("retrieved")]
        Retrieved,
        /// <summary>
        /// scale
        /// </summary>
        [XmlEnum("scale")]
        Scale,
        /// <summary>
        /// version
        /// </summary>
        [XmlEnum("version")]
        Version
    }
}