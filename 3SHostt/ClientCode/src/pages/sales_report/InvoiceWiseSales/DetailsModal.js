import {
    Button,
    LightMode,
    Modal,
    ModalBody,
    ModalCloseButton,
    ModalContent,
    ModalFooter,
    ModalOverlay
} from '@chakra-ui/react';
import React from 'react';
import { useQuery } from 'react-query';
import { onTicketDetailsByTicketId } from '../../../services/sales-service';
import { commaFormat, getTableTotal } from '../../../services/_utils';
import { getFormattedDateTime } from '../../../services/_utils/formatTime';
import InvoiceExport from '../../shared/InvoiceExport';
import Loading from '../../shared/Loading';

const DetailsModal = ({ isOpen, onClose, ticketId }) => {
    const onSuccess = (data) => {};
    const onError = (err) => console.log(err);
    const { data, isLoading } = useQuery(
        ['ticketDetails', ticketId],
        () => onTicketDetailsByTicketId(ticketId),
        { onSuccess, onError }
    );
    const { Calculations, Orders, Payments, Table1, TicketEntities } = data?.data || {};

    const style = {
        table: {
            width: '100%',
            margin: '20px 10px ',
            borderCollapse: 'collapse',
            position: 'relative',
            color: 'black'
        },
        captionTable: {
            textAlign: 'center',
            width: '100%',
            fontSize: '14px',
            fontWeight: 'bold',
            marginBottom: '15px'
        },
        infoTable: {
            width: '100%',
            marginBottom: '15px'
        },
        ticketTable: {
            width: '100%',
            marginBottom: '15px'
        },
        summaryTable: {
            width: '100%',
            marginBottom: '15px'
        },
        footerTable: {
            width: '100%',
            position: 'absolute',
            bottom: '10px',
            left: '0',
            fontSize: '10px',
            textAlign: 'right'
        },
        th: {
            border: '1px solid gray',
            background: 'lightgray',
            fontWeight: 'bold',
            padding: '0px 5px',
            textAlign: 'center',
            fontSize: '9px'
        },
        td: {
            border: '1px solid gray',
            padding: '0px 5px',
            fontSize: '9px'
        }
    };

    return (
        <LightMode>
            <Modal isOpen={isOpen} onClose={onClose} size="full">
                <ModalOverlay />
                <ModalContent color="black">
                    <ModalCloseButton className="hidePrintArea" />

                    <ModalBody>
                        <InvoiceExport title="Ticket Reports">
                            {isLoading ? (
                                <Loading line={12} width="100%" />
                            ) : (
                                <table style={style.table} id="invoice-table-to-xls">
                                    <table style={style.captionTable}>
                                        <tr>
                                            <td colSpan="11">Ticket Reports</td>
                                        </tr>
                                    </table>

                                    <table style={style.infoTable}>
                                        {Table1?.map((item) => (
                                            <tr>
                                                <td></td>
                                                <td style={style.td} colSpan={3}>
                                                    Ticket No: {item?.TicketNo || '-'}
                                                </td>
                                                <td style={style.td} colSpan={3}>
                                                    Ticket Date: {item?.TicketDate || '-'}
                                                </td>
                                                <td style={style.td} colSpan={3}>
                                                    Delivery Date: {item?.DeliveryDate || '-'}
                                                </td>
                                                <td></td>
                                            </tr>
                                        ))}

                                        <tr>
                                            <td></td>
                                            {TicketEntities?.map((item, i) => (
                                                <td style={style.td} colSpan={3} key={i}>
                                                    {item?.EntityType || '-'} :
                                                    {item?.EntityName || '-'}
                                                </td>
                                            ))}

                                            <td style={style.td} colSpan={3}>
                                                OutletName :
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>

                                    <table style={style.ticketTable}>
                                        <tr>
                                            <td style={style.th}>Menu Item</td>
                                            <td style={style.th}>Created By</td>
                                            <td style={style.th}>Created Time</td>
                                            <td style={style.th}>Order Status</td>
                                            <td style={style.th}>Quantity</td>
                                            <td style={style.th}>Unit Price</td>
                                            <td style={style.th}>Total Price</td>
                                            <td style={style.th}>Prod. Cost (Fixed)</td>
                                            <td style={style.th}>Prod. Profit (Fixed)</td>
                                            <td style={style.th}>Prod. Cost (Recipe)</td>
                                            <td style={style.th}>Prod. Profit (Recipe)</td>
                                        </tr>

                                        {Orders?.map((order, i) => (
                                            <tr key={i}>
                                                <td style={style.td}>{order?.MenuItem || '-'}</td>
                                                <td style={style.td}>
                                                    {order?.CreatingUserName || '-'}
                                                </td>
                                                <td style={style.td}>
                                                    {getFormattedDateTime(order?.CreatedDateTime) ||
                                                        '-'}
                                                </td>
                                                <td style={style.td}>
                                                    {order?.OrderStates || '-'}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(order?.Quantity || 0)}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(order?.UnitPrice || 0)}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(order?.Price || 0)}
                                                </td>
                                                <td style={style.td}>
                                                    {commaFormat(order?.FixedProductionCost || 0)}
                                                </td>
                                                <td style={style.td}>-</td>
                                                <td style={style.td}>
                                                    {commaFormat(order?.UnitProductionCost || 0)}
                                                </td>
                                                <td style={style.td}>-</td>
                                            </tr>
                                        ))}

                                        <tr>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}></td>
                                            <td style={style.td}>Total</td>
                                            <td style={style.td}>
                                                {commaFormat(getTableTotal(Orders, 'Price'))}
                                            </td>
                                            <td style={style.td}>
                                                {commaFormat(
                                                    getTableTotal(Orders, 'FixedProductionCost')
                                                )}
                                            </td>
                                            <td style={style.td}>-</td>
                                            <td style={style.td}>
                                                {commaFormat(
                                                    getTableTotal(Orders, 'UnitProductionCost')
                                                )}
                                            </td>
                                            <td style={style.td}>-</td>
                                        </tr>
                                    </table>

                                    <table style={style.summaryTable}>
                                        {Calculations?.map((item, i) => (
                                            <tr key={i}>
                                                <td colSpan={5}></td>
                                                <td colSpan={3} style={style.td}>
                                                    {item?.Name || '-'}
                                                </td>
                                                <td colSpan={3} style={style.td}>
                                                    {commaFormat(item?.CalculationAmount || 0)}
                                                </td>
                                            </tr>
                                        ))}

                                        <tr>
                                            <td colSpan={5}></td>
                                            <td colSpan={3} style={style.td}>
                                                Total Tax :
                                            </td>
                                            {Table1?.map((item, i) => (
                                                <td colSpan={3} style={style.td} key={i}>
                                                    {commaFormat(item?.TotalTax || 0)}
                                                </td>
                                            ))}
                                        </tr>
                                        <tr>
                                            <td colSpan={5}></td>
                                            <td colSpan={3} style={style.td}>
                                                Ticket Total :
                                            </td>
                                            {Table1?.map((item, i) => (
                                                <td colSpan={3} style={style.td} key={i}>
                                                    {commaFormat(item?.TicketTotal || 0)}
                                                </td>
                                            ))}
                                        </tr>

                                        {Payments?.map((item, i) => (
                                            <tr key={i}>
                                                <td colSpan={5}></td>
                                                <td colSpan={3} style={style.td}>
                                                    {item?.Name} ({getFormattedDateTime(item?.Date)}
                                                    )
                                                </td>
                                                <td colSpan={3} style={style.td}>
                                                    {commaFormat(item?.Amount || 0)}
                                                </td>
                                            </tr>
                                        ))}

                                        <tr>
                                            <td colSpan={5}></td>
                                            <td colSpan={3} style={style.td}>
                                                Remaining Amount :
                                            </td>
                                            {Table1?.map((item, i) => (
                                                <td colSpan={3} style={style.td} key={i}>
                                                    {commaFormat(item?.RemainingAmount || 0)}
                                                </td>
                                            ))}
                                        </tr>
                                    </table>

                                    <table style={style.footerTable}>
                                        <tr>
                                            <td colSpan={11}>
                                                Print Date & Time:{' '}
                                                {getFormattedDateTime(new Date())}
                                            </td>
                                        </tr>
                                    </table>
                                </table>
                            )}
                        </InvoiceExport>
                    </ModalBody>

                    <ModalFooter className="hidePrintArea">
                        <Button variant="ghost" onClick={onClose}>
                            Close
                        </Button>
                    </ModalFooter>
                </ModalContent>
            </Modal>
        </LightMode>
    );
};

export default DetailsModal;
