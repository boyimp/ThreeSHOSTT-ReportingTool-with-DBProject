import React from 'react';
import { Button, Flex, Heading } from '@chakra-ui/react';
import { SiMicrosoftexcel } from 'react-icons/si';
import { VscFilePdf } from 'react-icons/vsc';
import { BsPrinter } from 'react-icons/bs';
import ReactHTMLTableToExcel from 'react-html-table-to-excel';
import { PDFExport, savePDF } from '@progress/kendo-react-pdf';

const InvoiceExport = ({ children, title, landscape }) => {
    const pdfExportInvoice = React.useRef(null);

    const exportPDFWithComponent = () => {
        if (pdfExportInvoice.current) {
            pdfExportInvoice.current.save();
        }
    };

    const printReport = () => {
        window.print();
    };

    return (
        <Flex direction="column" align="center" justify="center">
            <Flex mb="5" mt="1" className="hidePrintArea" color="black">
                <ReactHTMLTableToExcel
                    id="test-table-xls-button"
                    className="download-table-xls-button"
                    table="invoice-table-to-xls"
                    filename={title}
                    sheet={title}
                    buttonText={
                        <Button size="sm" mr="2" fontSize="19px" title="Excel Export">
                            <SiMicrosoftexcel />
                        </Button>
                    }
                />
                <Button
                    size="sm"
                    mr="2"
                    fontSize="19px"
                    title="PDF Export"
                    onClick={exportPDFWithComponent}>
                    <VscFilePdf />
                </Button>

                <Button size="sm" mr="2" fontSize="19px" title="Print" onClick={printReport}>
                    <BsPrinter />
                </Button>
            </Flex>

            <Flex border="1px dashed black" h="8.7in" w="6.2in" className="hidePrintBorder">
                <PDFExport
                    ref={pdfExportInvoice}
                    paperSize="A4"
                    fileName={title}
                    landscape={landscape}>
                    <Flex h="8.7in" w="6.2in" className="printable">
                        {children}
                    </Flex>
                </PDFExport>
            </Flex>
        </Flex>
    );
};

export default InvoiceExport;
