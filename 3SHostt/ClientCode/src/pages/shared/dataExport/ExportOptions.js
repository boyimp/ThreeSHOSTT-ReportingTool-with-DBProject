import React from 'react';
import { Button, Flex, Heading } from '@chakra-ui/react';
import { SiMicrosoftexcel } from 'react-icons/si';
import { VscFilePdf } from 'react-icons/vsc';
import ReactHTMLTableToExcel from 'react-html-table-to-excel';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import { exportTable } from './ExportStyle';
import { PDFExport } from '@progress/kendo-react-pdf';

const ExportOptions = ({
    children,
    title,
    fromDate,
    toDate,
    colSpan,
    scale,
    margin,
    landscape
}) => {
    const pdfExportComponent = React.useRef(null);

    const exportPDFWithComponent = () => {
        if (pdfExportComponent.current) {
            pdfExportComponent.current.save();
        }
    };

    return (
        <>
            <Flex mb="5" mt="1">
                <ReactHTMLTableToExcel
                    id="test-table-xls-button"
                    className="download-table-xls-button"
                    table="table-to-xls"
                    filename={title}
                    sheet={title}
                    buttonText={
                        <Button
                            size="sm"
                            mr="2"
                            fontSize="19px"
                            title="Excel Export"
                            color="white"
                            background="brand.400"
                            _hover={{ background: 'brand.500' }}
                            _active={{ background: 'brand.400' }}>
                            <SiMicrosoftexcel />
                        </Button>
                    }
                />

                <Button
                    size="sm"
                    mr="2"
                    fontSize="19px"
                    title="PDF Export"
                    onClick={exportPDFWithComponent}
                    color="white"
                    background="brand.400"
                    _hover={{ background: 'brand.500' }}
                    _active={{ background: 'brand.400' }}>
                    <VscFilePdf />
                </Button>
            </Flex>

            <Flex position="absolute" left="-99999px">
                <PDFExport
                    ref={pdfExportComponent}
                    paperSize="A4"
                    scale={scale}
                    margin={margin}
                    fileName={title}
                    landscape={landscape}>
                    <table id="table-to-xls" style={exportTable.mainTable}>
                        <tr style={exportTable.tr}>
                            <th colSpan={colSpan} style={exportTable.mainTitle}>
                                {title} <br />
                            </th>
                        </tr>

                        {fromDate && toDate && (
                            <tr style={exportTable.tr}>
                                <td colSpan={colSpan} style={exportTable.workPeriod}>
                                    Work period considered from {getFormattedDateTime(fromDate)} to{' '}
                                    {getFormattedDateTime(toDate)}
                                </td>
                            </tr>
                        )}

                        {children}

                        <tr style={exportTable.tr}>
                            <th colSpan={colSpan} style={exportTable.footer}>
                                Powered By 3S
                            </th>
                        </tr>
                    </table>
                </PDFExport>
            </Flex>
        </>
    );
};

export default ExportOptions;
